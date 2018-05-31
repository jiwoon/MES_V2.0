<template>
  <div class="main-body">
    <side-setting/>
    <router-view/>
  </div>
</template>

<script>
  import SideSetting from '../components/SideSetting'

  export default {
    name: "Main",
    components: {
      SideSetting
    }
  }
</script>

<style scoped>

</style>
