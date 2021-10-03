<template>
  <div class="styles-app">
    <slot
      name="default"
      v-bind:props="{ compiledStyle, outputStyleRecord, outputCompiledStyle }"
    >
    </slot>
    <div class="style-wrapper">
      <StyleBuilder
        :styleRecord="styleRecordInternal"
        :schema="schema"
        :media-item-url="mediaItemUrl"
        @on-styles-change="renderDocumentStyles"
      />
    </div>
  </div>
</template>

<script lang="ts">
/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
import { Component, Prop, Vue } from "vue-property-decorator";
import postcss from "postcss";
import addRootSelector from "postcss-add-root-selector";
import StyleBuilder from "./components/StyleBuilder.vue";
import IStylesChangeArgs from "./types/style-change-args";
import { ColorPicker, ColorPanel } from "one-colorpicker";
import { ISchema } from "./types/schema";
import { IStyleRecord } from "./types/style-record";
import { buildStyleRecord } from "./stylebuilder/style-builder";

Vue.use(ColorPanel);
Vue.use(ColorPicker);

@Component({
  name: "styles-app",
  components: {
    StyleBuilder,
  },
})
export default class StylesApp extends Vue {
  @Prop() private styleRecord!: IStyleRecord;
  @Prop() private schema!: ISchema;
  @Prop() private compiledStyle!: string;
  @Prop() private mediaItemUrl!: string;

  private outputCompiledStyle = "";
  private outputStyleRecord = "";
  private styleRecordInternal: IStyleRecord = {};

  created(): void {
    this.styleRecordInternal = buildStyleRecord(this.schema, this.styleRecord);
    this.outputStyleRecord = JSON.stringify(this.styleRecordInternal);

    if (this.compiledStyle) {
      this.outputCompiledStyle = this.compiledStyle;

      this.applyStyles(this.compiledStyle);
    }
  }

  public renderDocumentStyles(args: IStylesChangeArgs) {
    const style = args.styles.join("\n");
    this.applyStyles(style);

    this.outputCompiledStyle = style;
    this.outputStyleRecord = JSON.stringify(args.styleRecord);
    setTimeout(function () {
      document.dispatchEvent(new Event("contentpreview:render"));
    }, 100);
  }

  private applyStyles(style: string) {
    postcss([
      addRootSelector({ rootSelector: "[dir]:root[data-theme=default]" }),
    ])
      .process(style, { from: undefined })
      .then((result: any) => {
        const existings = document.querySelectorAll(".tnw-dynamic-style");
        existings.forEach((e) => {
          document.head.removeChild(e);
        });
        let styleNode: HTMLStyleElement = document.createElement("style");
        styleNode.classList.add("tnw-dynamic-style");
        styleNode.textContent = result.css;
        document.head.appendChild(styleNode);
      });
  }
}
</script>

<style lang="scss">
.styles-app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  display: flex;
  flex-direction: column;
}
.style-wrapper {
  display: flex;
  justify-content: space-between;
}
</style>
