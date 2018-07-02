<template>
  <div id="app">
    <router-view/>
  </div>

</template>

<script>

  export default {
    name: 'App',
    components: {},
    mounted: function () {
      console.log(process.env.NODE_ENV)
    }
  }
</script>

<style>
  #app {
  }

  body {
    background: url("/static/img/background.png") repeat;

  }

  html, body {
    height: 100%;
  }
</style>
