//---------------------------------------------------------------------------------------------
// This file was AUTO-GENERATED by "WCF Service Contracts" Xomega.Net generator.
//
// Manual CHANGES to this file WILL BE LOST when the code is regenerated.
//---------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Xomega.Framework;

namespace AdventureWorks.Services
{
    #region IShipMethodService

    ///<summary>
    /// Shipping company lookup table.
    ///</summary>
    [ServiceContract]
    public interface IShipMethodService
    {

        ///<summary>
        /// Reads a list of Ship Method objects based on the specified criteria.
        ///</summary>
        [OperationContract]
        [FaultContract(typeof(ErrorList))]
        IEnumerable<ShipMethod_ReadListOutput> ReadList();

    }
    #endregion

    #region ShipMethod_ReadListOutput structure

    ///<summary>
    /// The output structure of operation IShipMethodService.ReadList.
    ///</summary>
    [DataContract]
    public class ShipMethod_ReadListOutput
    {
        [DataMember]
        public int ShipMethodId { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
    #endregion

}