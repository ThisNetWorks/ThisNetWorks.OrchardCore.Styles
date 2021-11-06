<template>
  <div>
    <div class="form-group row" v-if="schema">
      <label class="col-md-3 mb-0 text-left">{{ displayText }}</label>
      <div class="col-md-9">
        <size-input v-model="sizeModel" :props="schema.props" />
      </div>
      <span
        class="offset-md-3 col-md-9 hint d-block text-left"
        v-if="schema.hint"
        >{{ schema.hint }}</span
      >
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

import SizeInput from "./SizeInput.vue";
import { ISizeUnitValue } from "../types/size-unit-value";
import { ISchemaComponent } from "../types/schema";

interface ISizeValue extends IStyleRecordComponentValue {
  size: ISizeUnitValue;
  compiled: string;
}

@Component({ name: "size-single", components: { SizeInput } })
export default class SizeSingle extends Vue {
  @Prop() private schemaKey!: string;
  @Prop() private value!: ISizeValue;
  @Prop() private schema!: ISchemaComponent;

  private sizeModel: ISizeUnitValue = this.value.size;
  private compiled: string = this.value.compiled;

  get sizeValue(): ISizeValue {
    return { size: this.sizeModel, compiled: this.compiled };
  }

  get displayText(): string {
    if (this.schema && this.schema.displayText) {
      return this.schema.displayText;
    } else {
      return this.schemaKey;
    }
  }

  @Emit()
  updateValue(sizeValue: ISizeValue): IStyleRecordOutput {
    return { schemaKey: this.schemaKey, value: sizeValue };
  }

  @Emit()
  onError(error: any) {
    return error;
  }

  @Watch("sizeModel")
  sizeChanged(): void {
    this.renderStyle();
  }

  mounted(): void {
    if (
      this.compiled === "" &&
      this.value?.size?.size &&
      this.value?.size?.unit
    ) {
      this.renderStyle();
    }
  }

  private renderStyle(): void {
    styleRenderer(this.schema.renderer, {
      name: this.schemaKey,
      value: this.sizeModel,
      schema: this.schema,
    })
      .then((res) => {
        this.compiled = res.css;
        this.updateValue(this.sizeValue);
      })
      .catch((error) => this.onError(error));
  }
}
</script>

<style lang="css" />
