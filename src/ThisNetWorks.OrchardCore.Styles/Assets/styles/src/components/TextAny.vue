<template>
  <div>
    <div class="form-group row" v-if="schema">
      <label class="col-md-3 mb-0 text-left">{{ displayText }}</label>
      <div class="col-md-9">
        <input class="form-control" type="text" v-model="text" />
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
import { ISchemaComponent } from "../types/schema";
import {
  IStyleRecordComponentValue,
  IStyleRecordOutput,
} from "../types/style-record";

interface ITextValue extends IStyleRecordComponentValue {
  text: string;
  compiled: string;
}

@Component({ name: "text-any" })
export default class TextAny extends Vue {
  @Prop() private schemaKey!: string;
  @Prop() private value!: ITextValue;
  @Prop() private schema!: ISchemaComponent;

  private text: string = this.value.text;
  private compiled: string = this.value.compiled;

  get textValue(): ITextValue {
    return { text: this.text, compiled: this.compiled };
  }

  get displayText(): string {
    if (this.schema && this.schema.displayText) {
      return this.schema.displayText;
    } else {
      return this.schemaKey;
    }
  }

  @Emit()
  updateValue(textValue: ITextValue): IStyleRecordOutput {
    return { schemaKey: this.schemaKey, value: textValue };
  }

  @Emit()
  onError(error: any) {
    return error;
  }

  @Watch("text")
  textChanged(): void {
    this.renderStyle();
  }

  mounted(): void {
    if (this.compiled === "" && this.value?.text) {
      this.renderStyle();
    }
  }

  private renderStyle() {
    styleRenderer(this.schema.renderer, {
      name: this.schemaKey,
      value: this.text,
      schema: this.schema,
    })
      .then((res) => {
        this.compiled = res;
        this.updateValue(this.textValue);
      })
      .catch((error) => this.onError(error));
  }
}
</script>

<style lang="css" />
