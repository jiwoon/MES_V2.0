<!--表单查看页面的表单详情 统一配置-->
<template>
  <div class="main-details mt-1 mb-3">
    <datatable
      v-bind="$data"
    ></datatable>
  </div>
</template>

<script>
  import {axiosFetch} from "../../../../utils/fetchData";
  import {mapGetters, mapActions} from 'vuex'
  import {setRouterConfig, routerUrl} from "../../../../config/tableApiConfig";
  import {errHandler} from "../../../../utils/errorHandler";

  export default {
    name: "Details",
    components: {},
    data() {
      return {
        fixHeaderAndSetBodyMaxHeight: 650,
        tblStyle: {
          'word-break': 'break-all',
          'table-layout': 'fixed'

        },
        HeaderSettings: false,
        pageSizeOptions: [20, 40, 80, 100],
        data: [],
        //srcData: [],
        columns: [],
        total: 0,
        query: {"limit": 20, "offset": 0},
        isPending: false,
        thisRouter: ''
      }
    },
    created() {
      this.init();
      this.thisFetch(this.$route.query);
      this.thisRouter = this.$route.query.type;
    },
    computed: {
      ...mapGetters([
        'tableRouterApi'
      ]),

    },
    watch: {
      $route: function (route) {
        this.init();
        this.setLoading(true);
        if (route.query.type) {
          let options = {
            url: routerUrl,
            data: {
              table: route.query.type,
              pageNo: 1,
              pageSize: 20
            }
          };
          this.fetchData(options);
          this.thisRouter = route.query.type;
        } else if (route.query.data) {
          this.fetchData(route.query);
          this.thisRouter = route.query.data.table;

        }

      },
      query: {
        handler(query) {
          this.setLoading(true);
          this.dataFilter(query);
        },
        deep: true
      }
    },
    mounted: function () {
    },
    methods: {
      ...mapActions(['setTableRouter', 'setLoading']),
      init: function () {
        this.data = [];
        //this.srcData = [];
        this.columns = [];
        this.total = 0;
        this.thisRouter = '';
        this.query = {"limit": 20, "offset": 0}
      },
      thisFetch: function (opt) {
        let options = {
          url: routerUrl,
          data: {
            table: opt.type,
            pageNo: 1,
            pageSize: 20
          }
        };
        this.fetchData(options)
      },
      fetchData: function (options) {
        let routerConfig = setRouterConfig(options.data.table);
        this.columns = routerConfig.data.dataColumns;
        if (!(options.data.table === 'Gps_OperRecord'
          || options.data.table === 'GpsTcData'
          || options.data.table === 'GpsSMT_TcData')) {
          options.data.descBy = 'TestTime'
        } else if (options.data.table === 'Gps_OperRecord') {
          options.data.descBy = 'OperTime'
        }
        if (!this.isPending) {
          this.isPending = true;
          axiosFetch(options).then(response => {
            this.setLoading(false);
            this.isPending = false;
            if (response.data.result === 200) {
              //this.srcData = response.data.data.list;
              //this.data = response.data.data.list.slice(this.query.offset, this.query.offset + this.query.limit);
              this.data = response.data.data.list;
              this.data.map((item, index) => {
                item.showId = index + 1 + this.query.offset;
              });
              this.total = response.data.data.totalRow
            } else {
              errHandler(response.data.result)
            }
          })
            .catch(err => {
              this.isPending = false;
              console.log(JSON.stringify(err));
              alert('请求超时，请刷新重试')
            })
        } else {
          this.setLoading(false)
        }
      },
      dataFilter: function () {
        let options = {
          url: routerUrl,
          data: {
            table: this.thisRouter,
          }
        };
        options.data.pageNo = this.query.offset / this.query.limit + 1;
        options.data.pageSize = this.query.limit;
        this.fetchData(options);
        //this.data = this.srcData.slice(this.query.offset, this.query.offset + this.query.limit);
        // this.data.map((item, index) => {
        //   item.showId = index + 1 + this.query.offset;
        // })
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
