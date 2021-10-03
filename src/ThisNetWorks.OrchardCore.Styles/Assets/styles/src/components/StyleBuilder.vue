<template>
  <div class="style-builder w-100">
    <!-- here we either display a single component or a nested style builder -->
    <ul class="list-group w-100">
      <li
        v-for="(record, key, index) in styleRecord"
        :key="index"
        :set="(iSchema = schema[key])"
        class="list-group-item"
      >
        <div
          class="style-component-container"
          v-if="iSchema && iSchema.component"
        >
          <component
            :is="iSchema.component"
            v-bind:schemaKey="key"
            v-bind:value="record"
            v-bind:schema="iSchema"
            v-bind:media-item-url="mediaItemUrl"
            @update-value="updateValue"
          ></component>
        </div>
        <div v-else-if="iSchema" class="style-builder-container">
          <div class="display-text" v-if="iSchema.displayText">
            {{ iSchema.displayText }}
          </div>
          <div class="style-builder-component">
            <component
              :is="'style-builder'"
              v-bind:styleRecord="record"
              v-bind:schema="iSchema.schema"
              v-bind:schemaKey="key"
              v-bind:media-item-url="mediaItemUrl"
              @on-styles-change="handleStylesChange"
            ></component>
            <div v-if="iSchema.example" class="style-example">
              <span v-html="iSchema.example"></span>
            </div>
          </div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
import { Component, Prop, Vue, Emit } from "vue-property-decorator";
import { ISchema } from "../types/schema";
import { IStyleRecord, IStyleRecordOutput } from "../types/style-record";
import IStylesChangeArgs from "../types/style-change-args";
import Color from "./Color.vue";
import SizeSingle from "./SizeSingle.vue";
import TextAny from "./TextAny.vue";
// Disabled till shortcode available
// import Media from "./Media.vue";
import { styleRenderer } from "../renderers/renderers";

@Component({
  name: "style-builder",
  components: {
    Color,
    SizeSingle,
    TextAny,
    // Media,
  },
})
export default class StyleBuilder extends Vue {
  @Prop() private styleRecord!: IStyleRecord;
  @Prop() private schema!: ISchema;
  @Prop() private schemaKey: string | undefined;
  @Prop() private mediaItemUrl!: string;

  public updateValue(val: IStyleRecordOutput) {
    const schemaKey = val.schemaKey;
    this.styleRecord[schemaKey] = val.value;
    const styles = Object.values(this.styleRecord)
      .filter((style) => style.compiled)
      .map((s) => s.compiled as string);

    const args: IStylesChangeArgs = {
      styles: styles,
      schemaKey: this.schemaKey,
      styleRecord: this.styleRecord,
    };

    this.onStylesChange(args);
  }

  public handleStylesChange(args: IStylesChangeArgs): void {
    if (args.schemaKey) {
      const schemaKey = args.schemaKey;
      const schema = this.schema[schemaKey];
      if (schema) {
        const renderer = schema.renderer;
        if (renderer) {
          styleRenderer(renderer, {
            name: schemaKey,
            value: args.styles,
            schema: schema,
          })
            .then((res) => {
              // Here we still want to store this on the stylerecord
              this.styleRecord[schemaKey].compiled = res;
              const resultArgs: IStylesChangeArgs = {
                styles: [res],
                schemaKey: this.schemaKey,
                styleRecord: this.styleRecord,
              };
              this.onStylesChange(resultArgs);
            })
            .catch((error) => this.onError(error));
        }
      }
    } else {
      this.onStylesChange(args);
    }
  }

  @Emit()
  onStylesChange(args: IStylesChangeArgs): IStylesChangeArgs {
    return args;
  }

  public onError(error: any) {
    console.log(error);
  }
}
</script>

<style scoped lang="scss">
.style-builder {
  display: flex;
  & .style-builder-container {
    & .display-text {
      text-align: left;
      margin-bottom: 1rem;
    }
    & .style-builder-component {
      display: flex;
    }
  }
  & .style-example {
    min-width: 225px;
    display: flex;
    margin-left: auto;
    margin-right: auto;
    align-items: center;
    justify-content: center;
  }
}
</style>
