namespace mod_proteus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Services;
    using ipam;
    using LukeSkywalker.IPNetwork;
    public class Proteus
    {
        public class ObjectTypes
        {
            public const string AliasRecord = "AliasRecord";
            public const string Configuration = "Configuration";
            public const string CustomOptionDef = "CustomOptionDef";
            public const string DenyMACPool = "DenyMACPool";
            public const string DeploymentScheduler = "DeploymentScheduler";
            public const string Device = "Device";
            public const string DeviceSubType = "DeviceSubType";
            public const string DeviceType = "DeviceType";
            public const string DHCPRange = "DHCPRange";
            public const string DHCPDeploymentRole = "DHCPDeploymentRole";
            public const string DHCPMatchClass = "DHCPMatchClass";
            public const string DHCPServiceOption = "DHCPServiceOption";
            public const string DHCPSubClass = "DHCPSubClass";
            public const string DHCPV4ClientOption = "DHCPV4ClientOption";
            public const string DNSDeploymentRole = "DNSDeploymentRole";
            public const string DNSOption = "DNSOption";
            public const string DNSSECSigningPolicy = "DNSSigningPolicy";
            public const string Entity = "Entity";
            public const string EnumNumber = "EnumNumber";
            public const string EnumZone = "EnumZone";
            public const string ExternalHost = "ExternalHost";
            public const string GenericRecord = "GenericRecord";
            public const string HINFORecord = "HINFORecord";
            public const string HostRecord = "HostRecord";
            public const string InternalRootZone = "InternalRootZone";
            public const string IP4Address = "IP4Address";
            public const string IP4Block = "IP4Block";
            public const string IP4IPGroup = "IP4IPGroup";
            public const string IP4Network = "IP4Network";
            public const string IP4NetworkTemplate = "IP4NetworkTemplate";
            public const string IP4ReconciliationPolicy = "IP4ReconciliationPolicy";
            public const string IP6Address = "IP6Address";
            public const string IP6Block = "IP6Block";
            public const string IP6Network = "IP6Network";
            public const string Kerberos = "Kerberos";
            public const string LDAP = "LDAP";
            public const string MACAddress = "MACAddress";
            public const string MACPool = "MACPool";
            public const string MXRecord = "MXRecord";
            public const string NAPTRRecord = "NAPTRRecord";
            public const string NetworkInterface = "NetworkInterface";
            public const string PublishedServerInterface = "PublishedServerInterface";
            public const string Radius = "Radius";
            public const string RecordWithLink = "RecordWithLink";
            public const string Server = "Server";
            public const string SRVRecord = "SRVRecord";
            public const string StartOfAuthority = "StartOfAuthority";
            public const string Tag = "Tag";
            public const string TagGroup = "TagGroup";
            public const string TFTPDeploymentRole = "TFTDeploymentRole";
            public const string TFTPFile = "TFTPFile";
            public const string TFTPFolder = "TFTPFolder";
            public const string TFTPGroup = "TFTPGroup";
            public const string TXTRecord = "TXTRecord";
            public const string User = "User";
            public const string UserGroup = "UserGroup";
            public const string VendorClientOption = "VendorClientOption";
            public const string VendorOptionDef = "VendorOptionDef";
            public const string VendorProfile = "VendorProfile";
            public const string View = "View";
            public const string VirtualInterface = "VirtualInterface";
            public const string Zone = "Zone";
            public const string ZoneTemplate = "ZoneTemplate";
        }
        public class VirtualIPStack
        {
            public string ipaddress { get; set; }
            public string netmask { get; set; }
            public string gateway { get; set; }
            public string hostname { get; set; }
            public string suffix { get; set; }
            public string fqdn { get; set; }
            public long dnsViewid { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Credential"></param>
        /// <param name="wsdlPath"></param>
        /// <returns></returns>
        public static ProteusAPI Connect(NetworkCredential Credential, string wsdlPath)
        {
            try
            {
                ProteusAPI ipam = new ProteusAPI();
                string Username = Credential.UserName;
                string Password = Credential.Password;
                ipam.CookieContainer = new CookieContainer();
                ipam.Url = wsdlPath;
                ipam.login(Username, Password);
                return ipam;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Credential"></param>
        /// <param name="wsdlPath"></param>
        /// <param name="NetworkIdentity"></param>
        /// <returns></returns>
        public static APIEntity GetIp4Network(NetworkCredential Credential, string wsdlPath, string NetworkIdentity)
        {
            try
            {
                ProteusAPI proxy = Connect(Credential, wsdlPath);
                APIEntity[] ip4Networks = GetObjects(proxy, NetworkIdentity, ObjectTypes.IP4Network, 0, 10);
                if (ip4Networks.Count<APIEntity>() == 1)
                {
                    return ip4Networks[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static APIEntity GetIp4Network(ProteusAPI wsdlProxy, long EntityId)
        {
            APIEntity Entity = null;
            try
            {
                Entity = wsdlProxy.getEntityById(EntityId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Entity;
        }
        public static APIEntity GetIp4Network(ProteusAPI wsdProxy, string Name, long EntityId)
        {
            APIEntity Entity = null;
            try
            {
                APIEntity Parent = wsdProxy.getParent(EntityId);
                Entity = wsdProxy.getEntityByName(Parent.id, Name, ObjectTypes.IP4Network);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Entity;
        }
        public static VirtualIPStack GetIpStack(NetworkCredential Credential, string wsdlPath, long EntityId, string EntityName)
        {
            VirtualIPStack ipStack = new VirtualIPStack();
            ProteusAPI proxy = Connect(Credential, wsdlPath);
            APIEntity ipNetwork = GetIp4Network(proxy, EntityName, EntityId);
            IPNetwork vlan = ParseCidr(ipNetwork);
            string ipAddress = GetNextIp4Address(Credential, wsdlPath, ipNetwork.id);
            if (ipAddress == "")
            {
                //
                // ZOMG network is full
                //
                // we need to quit now
                //
            }
            else
            {
                ipStack.ipaddress = ipAddress;
            }
            ipStack.netmask = vlan.Netmask.ToString();
            ipStack.gateway = vlan.LastUsable.ToString();
            return ipStack;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Credential"></param>
        /// <param name="wsdlPath"></param>
        /// <param name="View"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static APIEntity GetDnsView(NetworkCredential Credential, string wsdlPath, string View, string Parent)
        {
            try
            {
                ProteusAPI proxy = Connect(Credential, wsdlPath);
                APIEntity[] Views = GetObjects(proxy, View, ObjectTypes.View, 0, 100);
                foreach (APIEntity dnsView in Views)
                {
                    APIEntity ParentView = proxy.getParent(dnsView.id);
                    if (ParentView.name == Parent)
                    {
                        return dnsView;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Credential"></param>
        /// <param name="wsdlPath"></param>
        /// <param name="NetworkID"></param>
        /// <returns></returns>
        public static string GetNextIp4Address(NetworkCredential Credential, string wsdlPath, long NetworkID)
        {
            try
            {
                ProteusAPI proxy = Connect(Credential, wsdlPath);
                return proxy.getNextAvailableIP4Address(NetworkID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Credential"></param>
        /// <param name="wsdlPath"></param>
        /// <param name="DnsView"></param>
        /// <param name="IpStack"></param>
        /// <param name="TTL"></param>
        /// <param name="Comments"></param>
        /// <returns></returns>
        public static long AddHostRecord(NetworkCredential Credential, string wsdlPath, VirtualIPStack IpStack, int TTL, string Comments, string View, string Parent)
        {
            try
            {
                ProteusAPI proxy = Connect(Credential, wsdlPath);
                string AbsoluteName = IpStack.hostname + "." + IpStack.suffix;
                APIEntity DnsView = GetDnsView(Credential, wsdlPath, View, Parent);
                long DnsViewID = DnsView.id;
                long HostID = proxy.addHostRecord(DnsViewID, AbsoluteName, IpStack.ipaddress, TTL, Comments);
                return HostID;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Duplicate of another item")
                {
                    //
                    // Find out who has my information
                    //
                    ProteusAPI proxy = Connect(Credential, wsdlPath);
                    APIEntity[] DupeIps = proxy.searchByObjectTypes(IpStack.ipaddress, ObjectTypes.IP4Address, 0, 10);
                    APIEntity[] DupeNames = proxy.searchByObjectTypes(IpStack.hostname, ObjectTypes.HostRecord, 0, 10);
                    if (DupeIps.Count<APIEntity>() > 0)
                    {
                        Console.WriteLine("Duplicate IP");
                    }
                    if (DupeNames.Count<APIEntity>() > 0)
                    {
                        Console.WriteLine("Duplicate Name");
                    }
                }
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wsdlProxy"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static APIEntity[] GetObjects(ProteusAPI wsdlProxy, string keyword, string type, int start, int count)
        {
            APIEntity[] entities = wsdlProxy.searchByObjectTypes(keyword, type, start, count);
            return entities;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ip4Network"></param>
        /// <returns></returns>
        private static  IPNetwork ParseCidr(APIEntity Ip4Network)
        {

            char[] splitChar = { '|' };
            string[] PropertyArray = Ip4Network.properties.Split(splitChar);
            string[] CIDRArray = Array.FindAll(PropertyArray, s => s.Contains("CIDR"));
            string CIDRNotation = CIDRArray[0].Replace("CIDR=", "");
            IPNetwork vlan = IPNetwork.Parse(CIDRNotation);
            return vlan;
        }
    }
}
