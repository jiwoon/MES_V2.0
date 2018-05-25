<template>
  <div class="options-area">
    <div class="form-row">
      <!--<div class="form-group col pr-3" v-for="item in queryOptions">-->
      <!--<label :for="item.id">{{item.name}}</label>-->
      <!--<input type="text" class="form-control" :id="item.id" v-model="item.model">-->
      <!--</div>-->
      <div class="form-group row no-gutters pl-3 pr-3">
        <label for="pageSizeSelect">筛选条数：</label>
        <select id="pageSizeSelect" class="form-control" v-model="pageSize">
          <option value="100">100</option>
          <option value="400">400</option>
          <option value="2000" selected>2000</option>
          <option value="2147483647">2147483647</option>
        </select>
      </div>
      <div v-for="item in queryOptions" class="row no-gutters pl-3 pr-3">
        <component :opt="item" :is="item.type + '-comp'"></component>
      </div>

      <div class="form-group col row align-items-end">
        <a href="#" class="btn btn-primary ml-3" @click="thisFetch">查询</a>
      </div>
    </div>
  </div>
</template>

<script>
  import {mapGetters, mapActions} from 'vuex';
  import {setRouterConfig, routerUrl} from "../../../../config/apiConfig";
  import {axiosFetch} from "../../../../utils/fetchData";
  import {Datetime} from 'vue-datetime'
  import 'vue-datetime/dist/vue-datetime.css'

  export default {
    name: "Options",
    components: {
      'text-comp': {
        props: ['opt'],
        template: '<div class="form-group col pr-3"">\n' +
        '           <label :for="opt.id">{{opt.name}}</label>\n' +
        '           <input type="text" class="form-control" :id="opt.id" v-model="opt.model">\n' +
        '          </div>'
      },
      'date-comp': {
        props: ['opt'],
        components: {
          Datetime
        },
        template: '<div class="row">\n' +
        '    <div class="form-group col pr-3">\n' +
        '      <label>测试时间  从：</label>\n' +
        '      <datetime v-model="opt.modelFrom" type="datetime"/>\n' +
        '    </div>\n' +
        '    <div class="form-group col pr-3">\n' +
        '      <label>至：</label>\n' +
        '      <datetime v-model="opt.modelTo" type="datetime"/>\n' +
        '    </div>\n' +
        '  </div>'

      }
    },
    data() {
      return {
        pageSize: 2000,
        queryOptions: [],
        queryString: "",
        test: '123'
      }
    },
    mounted: function () {
      if (this.$store.state.routerApi !== 'default') {
        this.initForm(this.$store.state.routerApi)
      }
    },
    computed: {
      ...mapGetters([
        'routerApi'
      ]),
    },
    watch: {
      routerApi: function (val) {
        console.log(111)
        this.initForm(val);
      }
    },
    methods: {
      ...mapActions(['setLoading']),
      initForm: function (opt) {
        let routerConfig = setRouterConfig(opt);
        this.queryOptions = routerConfig.data.queryOptions;
      },
      createQueryString: function () {
        this.queryString = "";
        this.queryOptions.map((item, index) => {
          if (item.type === 'text') {
            if (item.model !== "") {
              if (index === 0) {
                this.queryString += (item.id + "=" + item.model)
              } else {
                this.queryString += ("&" + item.id + "=" + item.model)
              }

            } else {
              this.setLoading(false)
            }
          } else if (item.type === 'date') {
            if (item.modelFrom !== '' && item.modelTo !== '') {
              let tempFrom = item.modelFrom.replace('T', ' ').replace('Z', '');
              let tempTo = item.modelTo.replace('T', ' ').replace('Z', '');
              if (this.compareDate(tempFrom, tempTo) >= 0) {
                if (index === 0) {
                  this.queryString += (item.id + '>=' + tempFrom + '&' + item.id + '<=' + tempTo)
                } else {
                  this.queryString += ('&' + item.id + '>=' + tempFrom + '&' + item.id + '<=' + tempTo)
                }
              } else {
                alert('日期格式错误');
                this.setLoading(false)
              }
            }
          }

        })
      },
      fetchData: function () {
        let options = {
          url: routerUrl,
          data: {
            table: this.$store.state.routerApi,
            pageNo: 1,
            pageSize: this.pageSize,
            filter: this.queryString
          }
        };
        //this.setRouter(obj.type);
        this.setLoading(true);
        this.$router.push({
          path: '/main/details',
          query: options
        }, () => {

        })

      },
      thisFetch: function () {
        this.createQueryString();
        this.fetchData()
      },
      compareDate: function (dateFrom, dateTo) {
        let compFrom = new Date(dateFrom);
        let compTo = new Date(dateTo);
        return (compTo - compFrom);
      }
    }
  }
</script>

<style scoped>
  .options-area {
    background: #fff;
    border: 1px solid #eeeeee;
    border-radius: 8px;
    padding: 10px;
  }
</style>
