<!--登录组件-->
<template>
  <div>
    <page-header/>

    <div class="login-container container justify-content-center" :style="{height: pageHeight + 'px'}">
      <div class="login-panel align-self-center col col-sm-10 col-md-5">
        <form @submit.prevent="loginSubmit">
          <div class="form-group mb-4 mt-4">
            <label for="login-username">用户名</label>
            <input type="text" id="login-username" class="form-control" placeholder="用户名" v-model="loginInfos.userName">
          </div>
          <div class="form-group mb-2">
            <label for="login-password">密 码</label>
            <input type="password" id="login-password" class="form-control" placeholder="密码"
                   v-model="loginInfos.password">
          </div>
          <!--<div class="form-check mb-2">-->
          <!--<input type="checkbox" class="form-check-input" id="login-check" v-model="loginInfos.checked">-->
          <!--<label class="form-check-label" for="login-check">干点啥</label>-->
          <!--</div>-->
          <div class="form-group mb-4 justify-content-center">
            <input type="submit" class="btn btn-primary" value="登录" style="width: 100%">
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
  import PageHeader from '../../components/PageHeader'
  import {axiosFetch} from "../../utils/fetchData";
  import {loginUrl} from "../../config/globalUrl";
  import {mapActions} from "vuex";
  import {errHandler} from "../../utils/errorHandler";

  export default {
    name: "Login",
    components: {
      PageHeader
    },
    data() {
      return {
        pageHeight: 0,
        isPending: false,
        loginInfos: {
          "userName": "",
          "password": "",
          //"#TOKEN#": ""
          //checked: false
        },
        token: ''
      }
    },
    mounted: function () {
      /*动态处理页面高度*/
      this.pageHeightCalc();
      window.onresize = () => {
        this.pageHeightCalc();
      };
      this.token = (localStorage.getItem('token'))
        ? localStorage.getItem('token')
        : '';
    },
    methods: {
      ...mapActions(['setLoginToken']),
      pageHeightCalc: function () {
        this.pageHeight = document.body.clientHeight - 200;
      },
      /*登录处理*/

      loginSubmit: function () {
        if (!this.isPending) {
          this.isPending = true;
          let options = {
            url: loginUrl,
            data: this.loginInfos
          };
          axiosFetch(options).then(res => {
            this.isPending = false;
            if (res.data.result === 200) {
              localStorage.setItem('token', res.data.data["#TOKEN#"]);
              this.setLoginToken(localStorage.getItem('token'));
              this.$router.replace('/');
            } else if (res.data.result === 412) {
              alert("请检查用户名或密码")
            } else if (res.data.result === 400) {
              alert("请勿重复登录");
              this.$router.replace('/table')
            } else {
              errHandler(res.data.result)
            }
          }).catch(err => {
            this.isPending = false;
            console.log(JSON.stringify(err));
            alert(err);
          })
        }
      }
    }
  }
</script>

<style scoped>
  .login-container {
    width: 100%;
    display: flex;
  }

  .login-panel {
    background: #fff;
    border: 1px solid #999;
    border-radius: 5px;
  }
</style>
