import axios from 'axios';
import {setLoginToken} from "../store/actions";
import {token} from "../store/getters";
import router from '../router';
import store from '../store'

axios.defaults.timeout = 5000;
axios.defaults.baseURL = 'http://10.10.11.109:8888/mes_server/';

axios.interceptors.request.use(
  config => {
    if (token) {
      config.headers.Authorization = `${store.state.token}`;
      config.headers['Content-Type'] =  'application/x-www-form-urlencoded; charset=UTF-8'

      console.log(config)
    }
    return config;
  },
  error => {
    return Promise.reject(error)
  }
);


axios.interceptors.response.use(
  res => {
    return res
  },
  error => {
    if (error.response) {
      console.log(JSON.stringify(error))
    }
    return Promise.reject(JSON.stringify(error))
  }
);

export default axios;
