<template>
  <div class="style-color form-group" v-if="schema">
    <div class="style-color__picker">
      <label class="mb-0">{{ displayText }}</label>
      <div class="pl-3">
        <color-picker
          :position="position"
          v-model="hexColor"
          @change="onPickerChange"
          class="border"
        ></color-picker>
      </div>
    </div>
    <span v-if="schema.hint" class="style-color__hint hint">{{
      schema.hint
    }}</span>
  </div>
</template>

<script lang="ts">
/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
import { Component, Prop, Vue, Emit } from "vue-property-decorator";

import rgbHex from "rgb-hex";
import hexRgb from "hex-rgb";
import { styleRenderer } from "../renderers/renderers";
import {
  IStyleRecordComponentValue,
  IStyleRecordOutput,
} from "../types/style-record";
import { ISchemaComponent } from "../types/schema";

interface IColor {
  hex: string;
  rgb: string;
}

interface IColorValue extends IStyleRecordComponentValue {
  color: IColor;
  compiled: string;
}

@Component({ name: "color" })
export default class Color extends Vue {
  @Prop() private schemaKey!: string;
  @Prop() private value!: IColorValue;
  @Prop() private schema!: ISchemaComponent;

  // Causes reactivity issues when passed as object.
  private hexColor = this.value.color.hex;
  private color: IColor = this.value.color;
  private compiled: string = this.value.compiled;
  private position = { left: "-272px" };

  get colorValue(): IColorValue {
    return { color: this.color, compiled: this.compiled };
  }

  get displayText() {
    if (this.schema && this.schema.displayText) {
      return this.schema.displayText;
    } else {
      return this.schemaKey;
    }
  }

  public onPickerChange(evt: string) {
    if (evt.startsWith("#")) {
      this.color.hex = evt;
      this.color.rgb = hexRgb(evt, { format: "css" });
    } else {
      this.color.hex = `#${rgbHex(evt)}`;
      this.color.rgb = evt;
    }
    this.renderStyle();
  }

  private renderStyle() {
    styleRenderer(this.schema.renderer, {
      name: this.schemaKey,
      value: this.color,
      schema: this.schema,
    })
      .then((res) => {
        this.compiled = res.css;
        this.updateValue(this.colorValue);
      })
      .catch((error) => this.onError(error));
  }

  mounted(): void {
    if (
      this.compiled === "" &&
      this.value?.color?.hex &&
      this.value?.color?.rgb
    ) {
      this.renderStyle();
    }
  }

  @Emit()
  updateValue(colorValue: IColorValue): IStyleRecordOutput {
    return { schemaKey: this.schemaKey, value: colorValue };
  }

  @Emit()
  onError(error: any) {
    return error;
  }
}
</script>

<style lang="scss">
.style-color {
  &__picker {
    display: flex;
    align-items: center;
    justify-content: space-between;
    & .one-colorpicker .one-colorpanel {
      z-index: 1000;
    }
  }
  &__hint {
    display: block;
    text-align: left;
  }
}
</style>
