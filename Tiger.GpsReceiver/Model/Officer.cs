using System;

namespace Tiger.GpsReceiver.Model
{
    [System.Runtime.Serialization.DataContract(Name = "GpsDeviceOfficer", Namespace = "http://www.tigerhz.com/GpsReceiver/model/")]
    public class Officer
    {
        private string _DeviceID;
        ///<summary>
        /// 
        ///</summary>
        [System.Runtime.Serialization.DataMember(Name = "DeviceID")]
        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }

        private string _OfficerID;
        ///<summary>
        /// 
        ///</summary>
        [System.Runtime.Serialization.DataMember(Name = "OfficerID")]
        public string OfficerID
        {
            get { return _OfficerID; }
            set { _OfficerID = value; }
        }

        private string _CarNum;
        ///<summary>
        /// 
        ///</summary>
        [System.Runtime.Serialization.DataMember(Name = "CarNum")]
        public string CarNum
        {
            get { return _CarNum; }
            set { _CarNum = value; }
        }
    }
}
