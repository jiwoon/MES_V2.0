/*报表统一配置页面*/
let url;
if (process.env.NODE_ENV === 'production') {
  url = window.g.API_URL
} else {
  url = window.g.LOCAL_URL
}
export const routerUrl = url + "/mes_server/report/select";

//export const routerUrl = "http://10.10.11.109:8080/mes_server/report/select";
export const setRouterConfig = (name) => {
  if (name === 'GpsTcData' || name === 'GpsSMT_TcData') {
    return {
      data: ROUTER_CONFIG.GpsTcData
    };
  } else if (name === 'Gps_AutoTest_Result2' || name === 'Gps_AutoTest_Result' || name === 'Gps_CoupleTest_Result'){
    return {
      data: ROUTER_CONFIG.GPS_AutoTest_Result
    }
  } else if (name === 'Gps_ParamDownload_Result') {
    return {
      data: ROUTER_CONFIG.Gps_ParamDownload_Result
    }
  } else if (name === 'Gps_CartonBoxTwenty_Result') {
    return {
      data: ROUTER_CONFIG.GPS_CartonBox_Result
    }
  } else if (name === 'Gps_OperRecord') {
    return {
      data: ROUTER_CONFIG.Gps_OperRecord
    }
  } else if (name === 'DataRelative_BAT') {
    return {
      data: ROUTER_CONFIG.DataRelative_BAT
    }
  } else if (name === 'DataRelative_VIP') {
    return {
      data: ROUTER_CONFIG.DataRelative_VIP
    }
  } else if (name === 'DataRelativeSheet') {
    return {
      data: ROUTER_CONFIG.DataRelative_Sheet
    }
  }
};

const ROUTER_CONFIG = {
  GpsTcData: {
    queryOptions: [
      {
        id: 'SN',
        name: 'SN号',
        model: '',
        type: 'text'
      }
    ],
    dataColumns: [
      {field: 'showId', title: '序号', colStyle: {'width': '70px'}},
      {field: 'SN', title: 'SN号', colStyle: {'width': '260px'}},
      {field: 'FixMode', title: '定位方式', colStyle: {'width': '60px'}},
      {field: 'GpsDb_0', title: 'GpsDb信息_1', colStyle: {'width': '60px'}},
      {field: 'GpsDb_1', title: 'GpsDb信息_2', colStyle: {'width': '60px'}},
      {field: 'GpsDb_2', title: 'GpsDb信息_3', colStyle: {'width': '60px'}},
      {field: 'GpsDb_3', title: 'GpsDb信息_4', colStyle: {'width': '60px'}},
      {field: 'GpsDb_4', title: 'GpsDb信息_5', colStyle: {'width': '60px'}},
      {field: 'GpsDb_5', title: 'GpsDb信息_6', colStyle: {'width': '60px'}},
      {field: 'GpsDb_6', title: 'GpsDb信息_7', colStyle: {'width': '60px'}},
      {field: 'GpsDb_7', title: 'GpsDb信息_8', colStyle: {'width': '60px'}},
      {field: 'GpsDb_8', title: 'GpsDb信息_9', colStyle: {'width': '60px'}},
      {field: 'GpsDb_9', title: 'GpsDb信息_10', colStyle: {'width': '60px'}},
      {field: 'GpsDb_10', title: 'GpsDb信息_11', colStyle: {'width': '60px'}},
      {field: 'GpsDb_11', title: 'GpsDb信息_12', colStyle: {'width': '60px'}},
    ]
  },


  GPS_AutoTest_Result: {
    queryOptions: [
      {
        id: 'SN',
        name: 'SN号',
        model: '',
        type: 'text'
      },
      {
        id: 'IMEI',
        name: 'IMEI',
        model: '',
        type: 'text'
      },
      {
        id: 'TestTime',
        name: '测试时间',
        modelFrom: '',
        modelTo: '',
        type: 'date'
      }
    ],
    dataColumns: [
      {field: 'showId', title: '序号', colStyle: {'width': '70px'},},
      {field: 'SN', title: 'SN号', colStyle: {'width': '260px'}},
      {field: 'IMEI', title: 'IMEI号', colStyle: {'width': '135px'}},
      {field: 'ZhiDan', title: '制单号',  colStyle: {'width': '135px'}},
      {field: 'SoftModel', title: '机型',  colStyle: {'width': '120px'}},
      {field: 'Version', title: '软件版本', colStyle: {'width': '200px'}},
      {field: 'Result', title: '结果',  colStyle: {'width': '50px'}},
      {field: 'TesterId', title: '测试员',  colStyle: {'width': '70px'}},
      {field: 'Computer', title: '地址',  colStyle: {'width': '160px'}},
      {field: 'TestSetting', title: '测试配置',  colStyle: {'width': '400px'}},
      {field: 'TestTime', title: '测试时间', colStyle: {'width': '175px'}},
      {field: 'Remark', title: '备注',  colStyle: {'width': '50px'}}
    ]
  },
  Gps_ParamDownload_Result: {
    queryOptions: [
      {
        id: 'SN',
        name: 'SN号',
        model: '',
        type: 'text'
      },
      {
        id: 'TestTime',
        name: '测试时间',
        modelFrom: '',
        modelTo: '',
        type: 'date'
      }
    ],
    dataColumns: [
      {field: 'showId', title: '序号', colStyle: {'width': '70px'},},
      {field: 'SN', title: 'SN号', colStyle: {'width': '260px'}},
      {field: 'IMEI', title: 'IMEI号', colStyle: {'width': '135px'}},
      {field: 'SoftModel', title: '机型',  colStyle: {'width': '120px'}},
      {field: 'Version', title: '软件版本', colStyle: {'width': '200px'}},
      {field: 'Result', title: '结果',  colStyle: {'width': '50px'}},
      {field: 'TesterId', title: '测试员',  colStyle: {'width': '70px'}},
      {field: 'Computer', title: '地址',  colStyle: {'width': '50px'}},
      {field: 'TestSetting', title: '测试配置',  colStyle: {'width': '80px'}},
      {field: 'TestTime', title: '测试时间', colStyle: {'width': '175px'}},
      {field: 'Remark', title: '备注',  colStyle: {'width': '50px'}}
    ]
  },

  GPS_CartonBox_Result: {
    queryOptions: [
      {
        id: 'IMEI',
        name: 'IMEI号',
        model: '',
        type: 'text'
      },
      {
        id: 'TestTime',
        name: '测试时间',
        modelFrom: '',
        modelTo: '',
        type: 'date'
      }
    ],
    dataColumns: [
      {field: 'showId', title:'序号', colStyle: {'width': '70px'}},
      {field: 'BoxNo', title:'箱号', colStyle: {'width': '90px'}},
      {field: 'IMEI', title:'IMEI号', colStyle: {'width': '120px'}},
      {field: 'ZhiDan', title:'制单号', colStyle: {'width': '120px'}},
      {field: 'SoftModel', title:'机型', colStyle: {'width': '60px'}},
      {field: 'Version', title:'软件版本', colStyle: {'width': '180px'}},
      {field: 'ProductCode', title:'产品编号', colStyle: {'width': '180px'}},
      {field: 'Color', title:'颜色', colStyle: {'width': '60px'}},
      {field: 'Qty', title:'数量', colStyle: {'width': '60px'}},
      {field: 'Weight', title:'重量', colStyle: {'width': '60px'}},
      {field: 'Date', title:'日期', colStyle: {'width': '90px'}},
      {field: 'TACInfo', title:'前缀', colStyle: {'width': '60px'}},
      {field: 'CompanyName', title:'公司名称', colStyle: {'width': '60px'}},
      {field: 'TesterId', title:'测试员', colStyle: {'width': '60px'}},
      {field: 'TestTime', title:'测试时间', colStyle: {'width': '120px'}},
      {field: 'Remark1', title:'备注1', colStyle: {'width': '90px'}},
      {field: 'Remark2', title:'备注2', colStyle: {'width': '120px'}},
      {field: 'Remark3', title:'备注3', colStyle: {'width': '90px'}},
      {field: 'Remark4', title:'备注4', colStyle: {'width': '400px'}},
      {field: 'Remark5', title:'备注5', colStyle: {'width': '60px'}},
      {field: 'Computer', title:'计算机地址', colStyle: {'width': '120px'}},
    ]
  },

  Gps_OperRecord: {
    queryOptions: [],
    dataColumns: [
      {field: 'showId', title: '序号', colStyle: {'width': '70px'}},
      {field: 'OperName', title: '操作用户', colStyle: {'width': '100px'}},
      {field: 'OperContent', title: '操作事项', colStyle: {'width': '100px'}},
      {field: 'OperTime', title: '操作时间', colStyle: {'width': '100px'}},
      {field: 'OperDemo', title: 'OperDemo', colStyle: {'width': '100px'}},

    ]
  },
  DataRelative_BAT: {
    queryOptions: [
      {
        id: 'IMEI',
        name: 'IMEI号',
        model: '',
        type: 'text'
      },
      {
        id: 'TestTime',
        name: '测试时间',
        modelFrom: '',
        modelTo: '',
        type: 'date'
      }
    ],
    dataColumns: [
      {field: 'showId', title:'序号', colStyle: {'width': '70px'}},
      {field: 'IMEI', title:'IMEI号', colStyle: {'width': '120px'}},
      {field: 'BAT', title:'BAT号', colStyle: {'width': '120px'}},
      {field: 'ZhiDan', title:'制单号', colStyle: {'width': '60px'}},
      {field: 'TesterId', title:'测试员', colStyle: {'width': '60px'}},
      {field: 'TestTime', title:'测试时间', colStyle: {'width': '60px'}},
    ]
  },
  DataRelative_VIP: {
    queryOptions: [
      {
        id: 'IMEI',
        name: 'IMEI号',
        model: '',
        type: 'text'
      },
      {
        id: 'TestTime',
        name: '测试时间',
        modelFrom: '',
        modelTo: '',
        type: 'date'
      }
    ],
    dataColumns: [
      {field: 'showId', title:'序号', colStyle: {'width': '70px'}},
      {field: 'IMEI', title:'IMEI号', colStyle: {'width': '120px'}},
      {field: 'VIP', title:'VIP号', colStyle: {'width': '120px'}},
      {field: 'ZhiDan', title:'制单号', colStyle: {'width': '60px'}},
      {field: 'TesterId', title:'测试员', colStyle: {'width': '60px'}},
      {field: 'TestTime', title:'测试时间', colStyle: {'width': '60px'}},
    ]
  },
  DataRelative_Sheet: {
    queryOptions: [
      {
        id: 'TestTime',
        name: '测试时间',
        modelFrom: '',
        modelTo: '',
        type: 'date'
      }
    ],
    dataColumns: [
      {field: 'showId', title:'序号', colStyle: {'width': '70px'}},
      {field: 'IMEI1', title:'IMEI/SIM卡号', colStyle: {'width': '120px'}},
      {field: 'IMEI2', title:'SN号', colStyle: {'width': '120px'}},
      {field: 'IMEI3', title:'SIM卡号', colStyle: {'width': '120px'}},
      {field: 'IMEI4', title:'ICCID', colStyle: {'width': '120px'}},
      {field: 'IMEI5', title:'密码:智能锁ID', colStyle: {'width': '120px'}},
      {field: 'IMEI6', title:'蓝牙MAC', colStyle: {'width': '120px'}},
      {field: 'IMEI7', title:'设备号', colStyle: {'width': '120px'}},
      {field: 'IMEI8', title:'服务卡号', colStyle: {'width': '120px'}},
      {field: 'IMEI9', title:'电池序列号', colStyle: {'width': '120px'}},
      {field: 'IMEI10', title:'?号', colStyle: {'width': '120px'}},
      {field: 'IMEI11', title:'??号', colStyle: {'width': '120px'}},
      {field: 'IMEI12', title:'???号', colStyle: {'width': '120px'}},
      {field: 'ZhiDan', title:'制单号', colStyle: {'width': '60px'}},
      {field: 'TestTime', title:'测试时间', colStyle: {'width': '60px'}},
    ]
  },


}
