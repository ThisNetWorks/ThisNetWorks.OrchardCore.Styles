import Vue from "vue";
import StylesApp from "./StylesApp.vue";

Vue.config.productionTip = false;

document.querySelectorAll("[data-tnw-styles]").forEach((el) => {
  new Vue({
    components: {
      StylesApp,
    },
  }).$mount(`#${el.id}`);
});
