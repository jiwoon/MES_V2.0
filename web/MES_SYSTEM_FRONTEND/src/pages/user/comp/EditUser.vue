<template>
  <transition name="fade">
    <div class="update-panel" v-if="isAdding">
      <div class="update-panel-container form-row flex-column justify-content-between">
        <div class="form-row">
          <div class="form-row col-6 pl-2 pr-2">
            <label for="user-id" class="col-form-label">UUID:</label>
            <input type="text" id="user-id" class="form-control" v-model="userData.userId" disabled>
          </div>
          <div class="form-row col-6 pl-2 pr-2">
            <label for="user-des" class="col-form-label">用户描述:</label>
            <input type="text" id="user-des" class="form-control" v-model="userData.userDes">
          </div>
          <div class="form-row col-6 pl-2 pr-2">
            <label for="type-select" class="col-form-label">用户类型:</label>
            <select id="type-select" class="custom-select" v-model="userData.userType">
              <option value="" disabled selected>请选择</option>
              <option value="SuperAdmin">SuperAdmin</option>
              <option value="IPQC">IPQC</option>
            </select>
          </div>
          <div class="form-row col-6 pl-2 pr-2">
            <label for="plan-select" class="col-form-label">测试计划:</label>
            <select id="plan-select" class="custom-select" v-model="userData.userTestPlan">
              <option value="" disabled>请选择</option>
              <option value="2">2</option>
              <option value="3">3</option>
            </select>
          </div>
          <div class="form-row col-6 pl-2 pr-2">
            <label for="active-select" class="col-form-label">是否启用:</label>
            <select id="active-select" class="custom-select" v-model="userData.inService">
              <option value="" disabled>请选择</option>
              <option value="0">禁用</option>
              <option value="1">启用</option>
            </select>
          </div>
        </div>
        <div class="dropdown-divider"></div>
        <div class="form-row justify-content-around">
          <a class="btn btn-secondary col mr-1 text-white" @click="isAdding = !isAdding">取消</a>
          <a class="btn btn-primary col ml-1 text-white" @click="addSubmit">提交</a>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
  export default {
    name: "EditUser",
    props: ['row'],
    data() {
      return {
        isAdding: false,
        userData: {
          userId: '',
          userDes: '',
          userType: '',
          userTestPlan: '',
          inService: ''
        },
        isPending: false
      }
    },
    mounted() {
      console.log(this.row)
    }
  }
</script>

<style scoped>
  .update-panel {
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

  .update-panel-container {
    background: #ffffff;
    min-height: 220px;
    max-width: 600px;
    z-index: 102;
    border-radius: 10px;
    box-shadow: 3px 3px 20px 1px #bbb;
    padding: 30px 60px 10px 60px;
  }

  .fade-enter-active, .fade-leave-active {
    transition: opacity .5s;
  }

  .fade-enter, .fade-leave-to {
    opacity: 0;
  }
</style>
