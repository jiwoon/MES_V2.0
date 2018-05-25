<template>
  <div class="mt-3 mb-3">
    <nav>
      <div class="sidebar-items">
        <!--透传数据-->
        <div class="sidebar-title">
          <a class="subtitle" draggable="false" data-toggle="collapse" href="#collapsePenetrate" aria-expanded="false"
             aria-controls="collapsePenetrate">透传数据</a>
        </div>
        <div class="collapse show" id="collapsePenetrate">
          <div v-for="data in penetrateLib" @click="toggleState(data.name)">
            <a class="sidebar-link" href="#" @click="linkTo(data)" :class="activeItem === data.name ? 'active' : ''">{{data.name}}</a>
          </div>
        </div>
        <!--各工位测试结果-->
        <div class="sidebar-title">
          <a class="subtitle" draggable="false" data-toggle="collapse" href="#collapseTestResult" aria-expanded="false"
             aria-controls="collapseTestResult">各工位测试结果</a>
        </div>
        <div class="collapse" id="collapseTestResult">
          <div v-for="data in testResultLib" @click="toggleState(data.name)">
            <a class="sidebar-link" href="#" @click="linkTo(data)" :class="activeItem === data.name ? 'active' : ''">{{data.name}}</a>

          </div>
        </div>
        <!--绑定结果-->
        <div class="sidebar-title">
          <a class="subtitle" draggable="false" data-toggle="collapse" href="#collapseBindResult" aria-expanded="false"
             aria-controls="collapseBindResult">绑定结果</a>
        </div>
        <div class="collapse" id="collapseBindResult">
          <div v-for="data in bindResultLib" @click="toggleState(data.name)">
            <a class="sidebar-link" href="#" @click="linkTo(data)" :class="activeItem === data.name ? 'active' : ''">{{data.name}}</a>

          </div>
        </div>
        <!--卡通结果-->
        <div class="sidebar-title">
          <a class="subtitle" draggable="false" data-toggle="collapse" href="#collapseCartonResult"
             aria-expanded="false" aria-controls="collapseCartonResult">卡通结果</a>
        </div>
        <div class="collapse" id="collapseCartonResult">
          <div v-for="data in cartonResultLib" @click="toggleState(data.name)">
            <a class="sidebar-link" href="#" @click="linkTo(data)" :class="activeItem === data.name ? 'active' : ''">{{data.name}}</a>

          </div>
        </div>
        <!--操作记录-->
        <div class="sidebar-title">
          <a class="subtitle" draggable="false" data-toggle="collapse" href="#operationRecord" aria-expanded="false"
             aria-controls="operationRecord">操作记录</a>
        </div>
        <div class="collapse" id="operationRecord">
          <div v-for="data in operationRecord" @click="toggleState(data.name)">
            <a class="sidebar-link" href="#" @click="linkTo(data)" :class="activeItem === data.name ? 'active' : ''">{{data.name}}</a>

          </div>
        </div>
      </div>
    </nav>

  </div>
</template>

<script>
  import {mapGetters, mapActions} from 'vuex'

  export default {
    data() {
      return {


        //透传数据的子类
        penetrateLib: [
          {
            type: "GpsSMT_TcData",
            link: "/main/details",
            name: "SMT状态"
          },
          {
            type: "GpsTcData",
            link: "/main/details",
            name: "组装情况"
          },
          // {
          //   type: "",
          //   link: "/",
          //   name: "透传数据总报表"
          // }
        ],
        //测试结果的子类
        testResultLib: [
          {
            type: "Gps_AutoTest_Result2",
            link: "/main/details",
            name: "SMT功能测试"
          },
          {
            type: "Gps_AutoTest_Result",
            link: "/main/details",
            name: "组装功能测试"
          },
          {
            type: "Gps_CoupleTest_Result",
            link: "/main/details",
            name: "耦合测试"
          },
          {
            type: "Gps_ParamDownload_Result",
            link: "/main/details",
            name: "软件参数下载"
          },
          // {
          //   type: "",
          //   link: "/",
          //   name: "绑定结果总报表"
          // }
        ],
        cartonResultLib: [
          {
            type: "Gps_CartonBoxTwenty_Result",
            link: "/main/details",
            name: "卡通结果"
          }
        ],
        operationRecord: [
          {
            type: "Gps_OperRecord",
            link: "/main/details",
            name: "操作记录"
          }
        ],
        //绑定结果的子类
        bindResultLib: [
          {
            type: "DataRelative_BAT",
            link: "/main/details",
            name: "电池绑定"
          },
          {
            type: "DataRelative_VIP",
            link: "/main/details",
            name: "VIP绑定"
          },
          {
            type: "DataRelativeSheet",
            link: "/main/details",
            name: "SIM绑定"
          },
          // {
          //   type: "",
          //   link: "/",
          //   name: "总关系"
          // }
        ],
        //控制列表active状态，当前已激活的项目
        activeItem: ""

      }
    },
    mounted: function () {

    },
    computed: {
      ...mapGetters([
        'routerApi',
        'isLoading'
      ]),
    },
    methods: {
      ...mapActions(['setRouter', 'setLoading']),
      toggleState: function (item) {
        this.activeItem = item;

      },
      linkTo: function (obj) {
        if (this.$store.state.routerApi !== obj.type) {
          this.setRouter(obj.type);
          this.setLoading(true);
          this.$router.push({
            path: obj.link,
            query: {
              type: obj.type
            }
          }, () => {

          })
        }
      }
    }


  }
</script>

<style scoped>
  a {
    text-decoration: none;
    color: #000;
  }
  .sidebar {
  }

  .sidebar-items {
    /*border: 1px solid #eeeeee;*/
    /*border-top: none;*/
    /*border-bottom: none;*/
    border: none;
    height: 100%;
    /*border-radius: 8px;*/
  }

  .sidebar-title {
    height: 2em;
    line-height: 2em;
    font-size: 1.2em;
    font-weight: 500;
    padding-left: 0.5em;
    border-bottom: 1px solid #eeeeee;
    background-color: #458aff;
    color: #fff;
    border-radius: 8px;
  }

  .sidebar-title a {
    color: #fff;
  }

  .sidebar-link {
    text-decoration: none;
    display: block;
    height: 2em;
    line-height: 2em;
    padding-left: 1.4em;
    font-size: 1em;
    border-bottom: 1px solid #eeeeee;
    font-weight: normal;
    background: #fff;
    cursor: pointer;
  }
  .sidebar-link:hover {
    background-color: #8bdaff;
    color: #fff;
    border-radius: 5px;
  }
  .sidebar-items .active {
    background-color: #7bbfff;
    box-shadow: 2px 4px 10px 1px #e5e7eb;
    color: #fff;
    border-radius: 5px;
  }

  .subtitle {
    cursor: pointer;
    display: block;
    width: 100%;
    height: 100%;
  }
</style>
