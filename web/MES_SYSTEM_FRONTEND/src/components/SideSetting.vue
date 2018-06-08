<!--侧边功能导航-->
<template>
  <div class="side-setting">
    <div class="setting-container">
      <div class="icon-container" :class="activeItem === 'table' ? 'icon-active' : '' " @click="initData('table')">
        <div class="setting-icon">
          <icon name="table" scale="1.6" style="color: #fff;"></icon>
        </div>
        <span>报表</span>
      </div>
      <div class="icon-container" :class="activeItem === 'setting' ? 'icon-active' : ''" @click="initData('setting')">
        <div class="setting-icon">
          <icon name="toggles" scale="1.6" style="color: #fff;"></icon>
        </div>
        <span>配置</span>
      </div>
      <!--<div class="icon-container">-->
      <!--<div class="setting-icon"></div>-->
      <!--</div>-->
      <div class="icon-container mt-auto" @click="logout">
        <div class="setting-icon">
          <icon name="power" scale="1.6" style="color: #fff;"></icon>
        </div>
        <span>登出</span>
      </div>
    </div>
  </div>
</template>

<script>
  import {mapGetters, mapActions} from 'vuex'
  import {axiosFetch} from "../utils/fetchData";
  import {logoutUrl} from "../config/globalUrl";
  import {errHandler} from "../utils/errorHandler";

  export default {
    name: "SideSetting",
    data() {
      return {
        activeItem: 'table' //活动项目
      }
    },
    computed: {
      ...mapGetters(['routerIn', 'token'])
    },
    methods: {
      ...mapActions(['setTableRouter', 'setLoading', 'setLoginToken']),
      /*点击切换项目、路由导航*/
      initData: function (item) {
        this.toggleState(item);
        this.linkTo(item);
      },
      /*切换当前活动项目*/
      toggleState: function (val) {
        this.activeItem = val;
      },
      /*路由导航*/
      linkTo: function (val) {
        this.setTableRouter('default');
        this.$router.replace({
          path: '/' + val,
        })
      },
      logout: function () {
        let options = {
          url: logoutUrl,
          data: {}
        };
        axiosFetch(options).then(res => {
          if (res.data.result === 200 || res.data.result === 400) {
            this.setLoginToken('');
            localStorage.removeItem('token');
            window.location.href = '/#/login'
          } else {
            errHandler(res.data.result)
          }
        }).catch(err => {
          console.log(JSON.stringify(err));
        })

      }
    }
  }
</script>

<style scoped>
  .side-setting {
    width: 60px;
    background: #458aff;
    position: fixed;
    height: 100%;
    z-index: 10;

  }

  .setting-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    height: 100%;
  }

  .setting-icon {
    border-radius: 7px;
    width: 28px;
    height: 28px;
    background: #ffb85b;
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;

  }

  .icon-container span {
    display: inline-block;
    font-size: 12px;
    line-height: 16px;
    color: #ffffff;
  }

  .icon-container {
    display: flex;
    align-items: center;
    flex-direction: column;
    margin: 20px 0;
    padding: 5px 0;
    cursor: pointer;
    width: 100%;
  }

  .setting-container .icon-active {
    border-left: #a4efff 3px solid;
    background: #4f97ff;
  }
</style>
