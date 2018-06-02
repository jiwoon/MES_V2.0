<!--订单配置页面根组件-->
<template>
    <div>
      <options/>
      <table-details/>
      <edit-panel v-if="isEditing" :editData="editData" />
    </div>
</template>

<script>
  import Options from './comp/Options'
  import TableDetails from './comp/TableDetails'
  import EditPanel from './comp/EditPanel'
  import {mapGetters, mapActions} from 'vuex'
  export default {
    name: "OrderManage",
    data () {
      return {

      }
    },
    computed: {
      ...mapGetters(['isEditing', 'editData'])
    },
    components: {
      Options,
      TableDetails,
      EditPanel
    }
  }
</script>

<style scoped>

</style>
