<template>
    <div>
      <options/>
      <table-details/>
    </div>
</template>

<script>
  import Options from './Comp/Options'
  import TableDetails from './Comp/TableDetails'
  export default {
    name: "Details",
    components: {
      Options,
      TableDetails
    }
  }
</script>

<style scoped>

</style>
