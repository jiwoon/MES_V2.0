<!--订单配置页面表单-->
<template>
  <div class="main-details mt-1 mb-3">
    <datatable
      v-bind="$data"
    ></datatable>
  </div>
</template>

<script>
  import Qs from 'qs'
  import {axiosFetch} from "../../../../utils/fetchData";
  import {mapGetters, mapActions} from 'vuex'
  import {setRouterConfig, routerUrl} from "../../../../config/orderApiConfig";
  import EditOptions from './EditOptions';
  import EditPanel from './EditPanel';
  import {errHandler} from "../../../../utils/errorHandler";

  export default {
    name: "Details",
    props: ['row'],
    components: {
      EditOptions,
      EditPanel
    },
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
        srcData: [],
        columns: [],
        total: 0,
        query: {"limit": 20, "offset": 0},
        isPending: false
      }
    },
    created() {
      this.init();
      this.thisFetch(this.$route.query)
    },
    computed: {
      ...mapGetters([
        'tableRouterApi'
      ]),

    },
    watch: {
      $route: function (route) {
        this.init();
        if (route.query.type) {
          let options = {
            url: routerUrl,
            data: {
              pageNo: 1,
              pageSize: 2000,
              descBy: 'ProductDate'
            }
          };
          this.fetchData(options)
        } else if (!route.query.data) {
          this.thisFetch(route.query)
        } else {
          this.fetchData(route.query)
        }


      },
      query: {
        handler(query) {
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
        this.srcData = [];
        this.columns = [];
        this.total = 0;
      },
      thisFetch: function (opt) {
        let options = {
          url: routerUrl,
          data: {
            pageNo: 1,
            pageSize: 2000,
            descBy: 'ProductDate'
          }
        };
        this.fetchData(options)
      },
      fetchData: function (options) {
        let routerConfig = setRouterConfig('order_manage');
        this.columns = routerConfig.data.dataColumns;
        if (!this.isPending) {
          this.isPending = true;
          axiosFetch(options).then(response => {
            this.setLoading(false);
            this.isPending = false;
            if (response.data.result === 200) {
              this.srcData = response.data.data.list;
              this.data = response.data.data.list.slice(this.query.offset, this.query.offset + this.query.limit);
              this.data.map((item, index) => {
                item.showId = index + 1 + this.query.offset;
                switch (item.Status) {
                  case 0:
                    item.ShowStatus = '未开始';
                    break;
                  case 1:
                    item.ShowStatus = '进行中';
                    break;
                  case 2:
                    item.ShowStatus = '已完成';
                    break;
                  case 3:
                    item.ShowStatus = '已作废';
                    break;
                }
              });
              this.total = response.data.data.list.length
            } else {
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
      },
      dataFilter: function () {
        this.data = this.srcData.slice(this.query.offset, this.query.offset + this.query.limit);
        this.data.map((item, index) => {
          item.showId = index + 1 + this.query.offset;
          switch (item.Status) {
            case 0:
              item.ShowStatus = '未开始';
              break;
            case 1:
              item.ShowStatus = '进行中';
              break;
            case 2:
              item.ShowStatus = '已完成';
              break;
            case 3:
              item.ShowStatus = '已作废';
              break;
          }
        })
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
