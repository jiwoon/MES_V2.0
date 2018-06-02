import Qs from 'qs'
import axios from 'axios'
export const axiosFetch = (opt) =>{
  return axios({
    method: 'post',
    url: opt.url,
    timeout: 10000,
    headers: {
      'Content-type': 'application/x-www-form-urlencoded; charset=UTF-8'
    },
    data: Qs.stringify(opt.data)
  })
};
