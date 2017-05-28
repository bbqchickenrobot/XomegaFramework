//---------------------------------------------------------------------------------------------
// This file was AUTO-GENERATED by "TS Lookup Cache Loaders" Xomega.Net generator.
//
// Manual CHANGES to this file WILL BE LOST when the code is regenerated.
//---------------------------------------------------------------------------------------------

import { BusinessEntityAddress_ReadListOutput, IBusinessEntityAddressService } from 'ServiceContracts/Person/IBusinessEntityAddressService';
import { BaseLookupCacheLoader, Header, LookupTable, ErrorList } from 'xomega';

export class BusinessEntityAddressReadListCacheLoader extends BaseLookupCacheLoader {

    constructor(caseSensitive: boolean) {
        super(caseSensitive, 'business entity address');
    }

    protected loadRequest(): JQueryAjaxSettings {
        // overrde this method in a subclass and provide proper input value(s)!
        return this.getLoadRequest(null);
    }

    protected getLoadRequest(_businessEntityId: any): JQueryAjaxSettings {
        return IBusinessEntityAddressService.getReadListRequest(_businessEntityId);
    }

    public loadCache(tableType: string, updateCache: (table: LookupTable) => void) {
        let req: JQueryAjaxSettings = this.loadRequest();
        let cl = this;
        req.success = function (data) {
            let lkpData: { [key: string]: Header } = {};
            let rows: BusinessEntityAddress_ReadListOutput[] = data || [];
            for (let row of rows) {
                let h: Header = lkpData[row.AddressId] || new Header(tableType, row.AddressId, row.AddressType);
                h.addToAttribute('address line 1', row.AddressLine1);
                h.addToAttribute('address line 2', row.AddressLine2);
                h.addToAttribute('city', row.City);
                h.addToAttribute('state', row.State);
                h.addToAttribute('postal code', row.PostalCode);
                h.addToAttribute('country', row.Country);
                lkpData[row.AddressId] = h;
            }
            var tblData = Object.keys(lkpData).map(k => lkpData[k]);
            var tbl: LookupTable = new LookupTable(tableType, tblData, cl.caseSensitive);
            updateCache(tbl);
        };
        req.error = (jqXHR, textStatus, errorThrow) => {
            updateCache(LookupTable.fromErrors(tableType, ErrorList.fromErrorResponse(jqXHR, errorThrow)));
        };
        $.ajax(req);
    }
}