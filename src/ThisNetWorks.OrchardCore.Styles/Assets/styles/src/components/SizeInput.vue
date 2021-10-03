<template>
  <div class="size-input">
    <div class="form-row">
      <div class="col-12 col-md-7">
        <vue-numeric-input
          class="numeric-input"
          v-model="model"
          :step="step"
          className="form-control"
          align="center"
          controls-type="plusminus"
        />
      </div>
      <div class="col-12 col-md-5">
        <select
          :value="value.unit"
          v-on:input="input('unit', $event.target.value)"
          class="form-control"
        >
          <option
            v-for="unit in units"
            :key="unit.value"
            v-bind:value="unit.value"
          >
            {{ unit.displayText }}
          </option>
        </select>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
import { Component, Prop, Vue, Emit } from "vue-property-decorator";
import { ISizeUnitValue } from "../types/size-unit-value";
import { enumToDescriptedArray, IDescriptor } from "../types/enums";
import VueNumericInput from "vue-numeric-input";

enum SizeUnit {
  Rem = "rem",
  Px = "px",
  Cm = "cm",
  Mm = "mm",
  In = "in",
  Pt = "pt",
  Pc = "pc",
  Em = "em",
  Ex = "ex",
  Ch = "ch",
  Vw = "vw",
  Vh = "vh",
  VMin = "vmin",
  VMax = "vmax",
  Percentage = "%",
}

interface ISizeSchemaProps {
  mergeUnits?: boolean;
  step?: number;
  units?: IDescriptor[];
}

@Component({ name: "size-input", components: { VueNumericInput } })
export default class SizeInput extends Vue {
  @Prop() private value!: ISizeUnitValue;
  @Prop() private props: ISizeSchemaProps | undefined;

  get step(): number {
    const step = this.props?.step;
    if (step) {
      return step;
    }
    return 1;
  }

  get model(): number {
    return this.value.size;
  }

  set model(val: number) {
    this.value.size = val;
    this.input("size", val);
  }

  get units() {
    if (this.props) {
      const units = this.props?.units;
      if (units && units.length > 0) {
        if (this.props?.mergeUnits === false) {
          return units;
        } else {
          const defaultUnits = enumToDescriptedArray(SizeUnit);

          var values = new Set(units.map((d) => d.value));
          var merged = [
            ...units,
            ...defaultUnits.filter((d) => !values.has(d.value)),
          ];

          return merged;
        }
      }
    }

    return enumToDescriptedArray(SizeUnit);
  }

  @Emit()
  input(key: string, value: string | number): ISizeUnitValue {
    return { ...this.value, [key]: value };
  }
}
</script>

<style lang="scss">
.size-input {
  & .vue-numeric-input {
    height: unset;
    button {
      width: 2rem;
      box-shadow: unset;
      -webkit-box-shadow: unset;
      background: #ced4da;
    }
  }
}
</style>
