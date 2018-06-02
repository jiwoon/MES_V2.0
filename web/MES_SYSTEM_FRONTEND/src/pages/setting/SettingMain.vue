<!--配置页面总组件-->
<template>
  <div>
    <!--<Header/>-->
    <loading v-if="$store.state.isLoading"/>
    <div class="" style="margin-left: 60px;">
      <div class="row main-style" :style="sideBarStyle">
        <transition name="slide">
          <div class="side-bar" v-show="sideBarIsShow">
            <side-bar/>
          </div>
        </transition>
        <div class="mt-5 toggle-sidebar">
          <a class="btn btn-primary" @click="sideBarIsShow = !sideBarIsShow">|</a>
        </div>
        <div class="col router-style mt-3 mb-3">
          <router-view/>
        </div>
      </div>
    </div>

  </div>
</template>
<script>
  import Header from '../../components/PageHeader'
  import SideBar from './details/comp/SideBar'
  import Loading from '@/components/Loading'
  import SideSetting from '../../components/SideSetting'
  import debounce from 'lodash/debounce'

  export default {
    name: "Main",
    components: {
      Header,
      SideBar,
      Loading,
      SideSetting
    },
    data() {
      return {
        sideBarIsShow: true,
        sideBarStyle: {
          height: '0px'
        }
      }
    },
    mounted: function () {
      let _this = this;
      this.sideBarStyle.height = document.body.scrollHeight + 'px';
      window.addEventListener('resize', () => {
        this.sideBarStyle.height = document.body.scrollHeight + 'px';
      });
      window.addEventListener('scroll',
        debounce(() => {
          this.sideBarStyle.height = document.body.scrollHeight + 'px';
        }, 400)
      )

    },
    methods: {}
  }
</script>

<style scoped>
  @media (min-width: 640px) {
    .side-bar {
      width: 250px;
    }

    .router-style {
      min-width: 340px;
    }
  }

  .side-bar {
    position: relative;
    background: #fff;
    box-shadow: 5px 0 5px 0 #ddd;
    /*height: 100%;*/
    padding: 0 20px;
  }

  .toggle-sidebar {
    z-index: 10;
  }

  .toggle-sidebar a {
    color: #fff;
    box-shadow: 5px 0 5px 0 #ddd;
    border-radius: 0 10px 10px 0;
    padding-left: 5px;
    padding-right: 5px;
  }

  .main-style {
    margin: 0;
    height: 100%;
  }

  .slide-enter-active, .slide-leave-active {
    transform: translate3d(0, 0, 0);
    margin-left: 0;
    transition: all .5s;
  }

  .slide-enter, .slide-leave-to {
    transform: translate3d(0, 0, 0);
    margin-left: -250px;
  }
</style>
