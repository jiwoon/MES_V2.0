import Vue from 'vue'
import Router from 'vue-router'
import Main from '../pages/Main'
import TableMain from '../pages/table/TableMain'
import Login from '../pages/login/Login'

import TableModule from '../pages/table/details/TableModule'
import SettingMain from '../pages/setting/SettingMain'
import OrderManage from '../pages/setting/details/OrderManage'
Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      component: Main,
      children: [
        {
          path: '/table',
          name: 'Table',
          component: TableMain,
          children: [
            {
              path: 'details',
              component: TableModule
            }
          ]
        },
        {
          path: '/setting',
          name: 'Setting',
          component: SettingMain,
          children: [
            {
              path: 'order_manage',
              component: OrderManage
            }
          ]
        }
      ]
    },

    {
      path: '/login',
      name: 'Login',
      component: Login
    }
  ]
})
