namespace Proteus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Services;
    using ipam;
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
                Console.WriteLine(ipam.getSystemInfo());
                return ipam;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
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
        private static APIEntity[] GetObjects(ProteusAPI wsdlProxy, string keyword, string type, int start, int count)
        {
            APIEntity[] entities = wsdlProxy.searchByObjectTypes(keyword, type, start, count);
            return entities;
        }
    }
}
