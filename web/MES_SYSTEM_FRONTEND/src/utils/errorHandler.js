import router from '../router'

const resHandler = function (attr) {
  return new Promise((resolve, reject) => {
    if (attr.code === '200') {
      resolve(attr.data)
    } else {
      let error = {};
      switch (attr.code) {
        default:
          error.code = attr.code;
          error.msg = '未知错误';
          break;

      }
      reject(error)
    }
  })
}

export const errHandler = function (code) {
  switch (code) {
    case 400:
      alert("请求逻辑错误，请联系管理员");
      break;
    case 401:
      alert("权限不足");
      break;
    case 412:
      alert("操作错误");
      break;
    case 500:
      alert("服务器异常，请联系管理员");
      break;
    case 501:
      alert("未知错误，请联系管理员");
      break;
    default:
      break;
  }
};
