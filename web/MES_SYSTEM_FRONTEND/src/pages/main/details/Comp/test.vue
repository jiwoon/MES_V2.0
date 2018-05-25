<template>
  <div>
    <div class="form-group col pr-3">
      <label>从</label>
      <datetime v-model="opt.fromModel" :class="form-control"/>
    </div>
    <div class="form-group col pr-3">
      <label>至</label>
      <datetime v-model="opt.toModel" :class="form-control"/>
    </div>
  </div>
</template>

<script>
  export default {
    name: "test"
  }
</script>

<style scoped>

</style>
