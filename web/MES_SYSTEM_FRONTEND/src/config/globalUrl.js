let url;
if (process.env.NODE_ENV === 'production') {
  url = window.g.API_URL
} else {
  url = window.g.LOCAL_URL
}


export const loginUrl = url + '/mes_server/user/login';
export const logoutUrl = url + '/mes_server/user/logout';
