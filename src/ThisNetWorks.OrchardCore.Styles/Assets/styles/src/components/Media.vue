<template>
  <div>
    <div class="form-group row" v-if="schema">
      <div class="col-12 col-md-6">Media Item Lable:</div>
      <div class="col-12 col-md-6">
        <media-picker
          :path="path"
          @path-change="onPathChange"
          :mediaItemUrl="mediaItemUrl"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
import { Component, Prop, Vue, Emit, Watch } from "vue-property-decorator";
import { styleRenderer } from "../renderers/renderers";
import {
  IStyleRecordComponentValue,
  IStyleRecordOutput,
} from "../types/style-record";
import { ISchemaComponent } from "../types/schema";
import { IAssetValue } from "../types/asset-value";
import MediaPicker from "./MediaPicker.vue";

interface IMediaValue extends IStyleRecordComponentValue {
  asset: IAssetValue;
  compiled: string;
}

@Component({ name: "media", components: { MediaPicker } })
export default class Media extends Vue {
  @Prop() private schemaKey!: string;
  @Prop() private value!: IMediaValue;
  @Prop() private schema!: ISchemaComponent;
  @Prop() private mediaItemUrl!: string;

  private path: string = this.value.asset.path;
  private shortcode = this.value.asset.shortcode;
  private compiled: string = this.value.compiled;

  get assetValue(): IAssetValue {
    return { path: this.path, shortcode: this.shortcode };
  }

  get mediaValue(): IMediaValue {
    return { asset: this.assetValue, compiled: this.compiled };
  }

  get displayText(): string {
    if (this.schema && this.schema.displayText) {
      return this.schema.displayText;
    } else {
      return this.schemaKey;
    }
  }

  onPathChange(newPath: string) {
    this.path = newPath;
    this.shortcode = `[asset_url]${this.path}[/asset_url]`;
  }

  @Emit()
  updateValue(mediaValue: IMediaValue): IStyleRecordOutput {
    return { schemaKey: this.schemaKey, value: mediaValue };
  }

  @Emit()
  onError(error: any) {
    return error;
  }

  @Watch("path")
  pathChanged(): void {
    this.renderStyle();
  }

  mounted(): void {
    if (this.compiled === "" && this.value?.asset.shortcode) {
      this.renderStyle();
    }
  }

  private renderStyle(): void {
    styleRenderer(this.schema.renderer, {
      name: this.schemaKey,
      value: this.shortcode,
      schema: this.schema,
    })
      .then((res) => {
        this.compiled = res.css;
        this.updateValue(this.mediaValue);
      })
      .catch((error) => this.onError(error));
  }
}
</script>

<style lang="css" />
