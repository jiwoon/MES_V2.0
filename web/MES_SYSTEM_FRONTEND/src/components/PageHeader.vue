<!--顶栏组件-->

<template>
  <nav class="navbar navbar-light shadow-sm">
    <div class="container">

      <a class="navbar-brand" draggable="false">
        <span>
          <img src="/static/img/logo.png" class="nav-logo" draggable="false">
        </span>
        智能生产控制系统</a>
    </div>
  </nav>
</template>

<script>
  export default {}
</script>

<style scoped>
  .nav-logo {
    width: 56px;
  }

  .navbar-brand {
    font-size: 1.6em;
    color: #fff !important;
  }

  nav a:not, nav a:hover {
    color: #fff !important;
  }

  nav {
    background: #3875ff;
  }
</style>
