<template>
  <div class="main-details mt-1 mb-3">
    <datatable
      v-bind="$data"
    ></datatable>
  </div>
</template>

<script>
  import {userAddUrl, userUpdateUrl, userQueryUrl} from "../../../config/globalUrl";
  import {mapGetters, mapActions} from 'vuex'
  import {axiosFetch} from "../../../utils/fetchData";
  import {errHandler} from "../../../utils/errorHandler";
  import UserOperation from "./UserOperation"
  export default {
    name: "UserDetails",
    components: {
      UserOperation
    },
    data() {
      return {
        fixHeaderAndSetBodyMaxHeight: 650,
        tblStyle: {
          'word-break': 'break-all',
          'table-layout': 'fixed'
        },
        HeaderSettings: false,
        pageSizeOptions: [20, 40],
        data: [],
        columns: [
          {field: 'UserId', title: 'UUID', colStyle: {'width': '100px'}},
          {field: 'UserName', title: '用户名', colStyle: {'width': '100px'}},
          {field: 'UserType', title: '用户类型', colStyle: {'width': '100px'}},
          {field: 'LoginTime', title: '最后一次登录时间', colStyle: {'width': '100px'}},
          {field: 'InService', title: '是否启用', colStyle:{'width': '100px'}},
          {title: '操作', tdComp: 'UserOperation', colStyle: {'width': '100px'}}

        ],
        total: 0,
        query: {"limit": 20, "offset": 0},
        isPending: false
      }
    },
    created() {
      this.init();
    },
    mounted () {
      this.thisFetch(this.$route.query)

    },
    watch: {
      $route: function (val) {
        this.thisFetch(val.query)
      }
    },
    methods: {
      ...mapActions(['setLoading']),
      init: function () {
        this.data = [];
        this.total = 0;
        this.query = {"limit": 20, "offset": 0}
      },
      thisFetch: function (opt) {
        let options = {
          url: userQueryUrl,
          data: {
            table: 'Gps_User',
            pageNo: 1,
            pageSize: 20
          }
        };
        this.fetchData(options);
      },
      fetchData: function (opt) {
        if (!this.isPending) {
          this.isPending = true;
          axiosFetch(opt).then(response => {
            this.isPending = false;
            this.setLoading(false);
            if (response.data.result === 200) {
              this.data = response.data.data.list;
              this.total = response.data.data.totalRow;
            } else {
              this.isPending = false;
              errHandler(response.data.result)
            }
          })
            .catch(err => {
              this.isPending = false;
              console.log(JSON.stringify(err));
              alert('请求超时，清刷新重试')
            })
        } else {
          this.setLoading(false)
        }
      }
    }
  }
</script>

<style scoped>
  .main-details {
    background: #fff;
    border: 1px solid #eeeeee;
    border-radius: 8px;
    padding: 10px;
    min-height: 500px;
  }
</style>
