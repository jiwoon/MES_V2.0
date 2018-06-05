<!--订单编辑页面中的操作栏项目-->
<template>
  <div class="edit-options form-row">
    <div class="btn pl-1 pr-1" title="编辑" data-placement="top" @click="editThis(row)">
      <icon name="edit" scale="1.8"></icon>
    </div>
    <div class="btn pl-1 pr-1" title="复制" @click="copyFrom(row)">
      <icon name="copy" scale="1.8"></icon>
    </div>
    <div class="btn pl-1 pr-1" title="状态" @click="editStatus(row)">
      <icon name="menu" scale="1.8"></icon>
    </div>
    <div class="status-panel" v-if="isStatusPanel">
      <div class="status-panel-container form-row flex-column justify-content-between">
        <div class="form-row">
          <label for="status-select" class="col-form-label">状态更改:</label>
          <select id="status-select" class="custom-select"
                  v-model="thisRow.Status">
            <option value="" disabled>请选择</option>
            <option value="0">未开始</option>
            <option value="1">进行中</option>
            <option value="2">已完成</option>
            <option value="3">已作废</option>
          </select>
        </div>
        <div class="dropdown-divider"></div>
        <div class="form-row justify-content-around">
          <a class="btn btn-secondary col mr-1 text-white" @click="isStatusPanel = !isStatusPanel">取消</a>
          <a class="btn btn-primary col ml-1 text-white" @click="statusSubmit">提交</a>
        </div>
      </div>
    </div>
  </div>

</template>

<script>
  import {mapActions} from 'vuex'
  import {setRouterConfig, routerUrl} from "../../../../config/orderApiConfig";
  import {orderOperUrl} from "../../../../config/orderApiConfig";
  import {axiosFetch} from "../../../../utils/fetchData";
  import {errHandler} from "../../../../utils/errorHandler";

  export default {
    name: "td-Options",
    components: {},
    data() {
      return{
        isStatusPanel: false,
        thisRow: {}
      }
    },
    props: ['row'],
    methods: {
      ...mapActions(['setEditing', 'setEditData', 'setCopyData']),
      editThis: function (val) {
        let options = setRouterConfig('order_manage').data.dataColumns;
        let formData = [];
        options.map((item, index) => {
          if (item.field) {
            let data = {
              title: item.title,
              field: item.field,
              value: val[item.field]
            };
            formData.push(data)
          }
        });
        this.setEditData(formData);
        this.setEditing(true);
      },
      copyFrom: function (val) {
        let options = setRouterConfig('order_manage').data.dataColumns;
        let formData = [];
        options.map((item, index) => {
          if (item.field) {
            let data = {
              title: item.title,
              field: item.field,
              value: val[item.field]
            };
            formData.push(data)
          }
        });
        this.setCopyData(formData);
        this.setEditing(true)

      },
      editStatus: function (val) {
        this.isStatusPanel = true;
        this.thisRow = val;
      },
      statusSubmit: function () {
        if (this.thisRow.Status > 0) {
          let thisStatus;
          switch (this.thisRow.Status) {
            case '1':
              thisStatus = '/start';
              break;
            case '2':
              thisStatus = '/finish';
              break;
            case '3':
              thisStatus = '/cancel';
              break;
          }
          let options = {
            url: orderOperUrl + thisStatus,
            data: {
              key: this.thisRow.Id
            }
          };
          axiosFetch(options).then(res => {
            if (res.data.result === 200) {
              alert('更新成功');
              this.isStatusPanel = false;
              this.thisRow = {};
              let tempUrl = this.$route.fullPath;
              //console.log(this.$route.url)
              this.$router.replace('/_empty');
              this.$router.replace(tempUrl)
            } else {
              errHandler(res.data.result)
            }
          }).catch(err => {
            alert(err)
          })
        }
      }
    }
  }
</script>

<style scoped>
  .status-panel {
    position: fixed;
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;
    width: 100%;
    left: 0;
    top: 0;
    background: rgba(0, 0, 0, 0.1);
    z-index: 101;
  }

  .status-panel-container {
    background: #ffffff;
    height: 220px;
    width: 360px;
    z-index: 102;
    border-radius: 10px;
    box-shadow: 3px 3px 20px 1px #bbb;
    padding: 30px 60px 10px 60px;
  }
</style>
