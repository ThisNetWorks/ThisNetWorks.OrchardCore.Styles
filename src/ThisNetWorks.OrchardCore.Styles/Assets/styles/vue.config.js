const path = require("path");

module.exports = {
  filenameHashing: false,
  outputDir: path.resolve(__dirname, "../../wwwroot/dist"),
  css: {
    extract: true
  },
  chainWebpack: config => {
    config.resolve.alias
      .set('vue$', 'vue/dist/vue.esm.js');

    if (process.env.APP_ENV !== undefined && process.env.APP_ENV.trim() === "external" || process.env.NODE_ENV == 'production') {
      console.log('vue.js configured as external')
      config.externals({
        'vue': 'Vue',
        $: 'jquery',
        'jquery': 'jQuery',
        'bootstrap': 'bootstrap'
      });
    } else {
      console.log(`vue.js is bundled`);
    }
  }
}
