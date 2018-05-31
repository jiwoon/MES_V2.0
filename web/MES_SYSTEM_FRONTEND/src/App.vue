<template>
  <div id="app">
    <router-view/>
  </div>

</template>

<script>

  export default {
    name: 'App',
    components: {}
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
