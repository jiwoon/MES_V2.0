/*订单数据配置*/

export const orderOperUrl = 'http://10.10.11.109:8888/mes_server/order';
export const routerUrl = "http://10.10.11.109:8888/mes_server/order/select";

//export const routerUrl = "http://10.10.11.109:8080/mes_server/order/select";
export const setRouterConfig = (name) => {
  if (name === 'order_manage') {
    return {
      data: ROUTER_CONFIG.OrderManage
    };
  }
};

const ROUTER_CONFIG = {
  OrderManage: {
    queryOptions: [
      {
        id: 'ZhiDan',
        name: '制单号',
        model: '',
        type: 'text'
      }
    ],
    dataColumns: [
      {field: 'Id', title: '序号', visible:false},
      {field: 'showId', title: '序号', colStyle: {'width': '70px'}},
      {title: '操作', tdComp: 'EditOptions', colStyle: {'width': '100px'}},
      {field: 'ShowStatus', title: '状态', colStyle: {'width': '80px'}},
      {field: 'Status', title: '状态', colStyle: {'width': '80px'}, visible: false},
      {field: 'ZhiDan', title: '制单号', colStyle: {'width': '130px'}},
      {field: 'SoftModel', title: '型号', colStyle: {'width': '100px'}},
      {field: 'SN1', title: 'SN1', colStyle: {'width': '100px'}},
      {field: 'SN2', title: 'SN2', colStyle: {'width': '70px'}},
      {field: 'SN3', title: 'SN3', colStyle: {'width': '70px'}},
      {field: 'BoxNo1', title: '箱号1', colStyle: {'width': '70px'}},
      {field: 'BoxNo2', title: '箱号2', colStyle: {'width': '70px'}},
      {field: 'ProductDate', title: '生产日期', colStyle: {'width': '100px'}},
      {field: 'Color', title: '颜色', colStyle: {'width': '60px'}},
      {field: 'Weight', title: '重量', colStyle: {'width': '80px'}},
      {field: 'Qty', title: '数量', colStyle: {'width': '80px'}},
      {field: 'ProductNo', title: '产品编号', colStyle: {'width': '140px'}},
      {field: 'Version', title: '版本', colStyle: {'width': '140px'}},
      {field: 'IMEIStart', title: '起始IMEI号', colStyle: {'width': '150px'}},
      {field: 'IMEIEnd', title: '终止IMEI号', colStyle: {'width': '150px'}},
      {field: 'SIMStart', title: '起始SIM卡号', colStyle: {'width': '150px'}},
      {field: 'SIMEnd', title: '终止SIM卡号', colStyle: {'width': '150px'}},
      {field: 'BATStart', title: '起始BAT号', colStyle: {'width': '120px'}},
      {field: 'BATEnd', title: '终止BAT号', colStyle: {'width': '120px'}},
      {field: 'VIPStart', title: '起始VIP号', colStyle: {'width': '120px'}},
      {field: 'VIPEnd', title: '终止VIP号', colStyle: {'width': '120px'}},
      {field: 'IMEIRel', title: 'IMEI关联', colStyle: {'width': '60px'}},
      {field: 'TACInfo', title: 'TAC信息', colStyle: {'width': '100px'}},
      {field: 'CompanyName', title: '公司名', colStyle: {'width': '100px'}},
      {field: 'Remark1', title: '备注1', colStyle: {'width': '200px'}},
      {field: 'Remark2', title: '备注2', colStyle: {'width': '100px'}},
      {field: 'Remark3', title: '备注3', colStyle: {'width': '100px'}},
      {field: 'Remark4', title: '备注4', colStyle: {'width': '100px'}},
      {field: 'Remark5', title: '备注5', colStyle: {'width': '100px'}}
    ]
  },


}
