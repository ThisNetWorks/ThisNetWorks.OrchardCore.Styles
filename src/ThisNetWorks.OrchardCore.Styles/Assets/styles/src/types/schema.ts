/* eslint-disable @typescript-eslint/no-unused-vars */
import { IDynamic, isOfType } from "./dynamic";

const testSchema2: ISchema = {
  Button: {
    displayText: "The whole button (nested selectors)",
    hint: "The whole button",
    renderer: {
      name: "LiquidJs",
      template: ".my-btn { {%- for v in value %}{{ v }}{%- endfor%} }",
    },
    example: "<button class='btn my-btn'>Default</button",
    schema: {
      ButtonColor: {
        component: "Color",
        displayText: "Button text color",
        hint: "Pick a color for the button text",
        defaultValue: {
          color: {
            hex: "#8bc34a",
            rgb: "rgba(139, 195, 74, 1)",
          },
        },
        renderer: {
          name: "LiquidJs",
          template: "color: {{ value.hex }};",
        },
      },
    },
  },
};

const defaultValue: ISchemaComponentValue = {
  color: {
    hex: "#8bc34a",
    rgb: "rgba(139, 195, 74, 1)",
  },
};

const testSchemaRenderer: ILiquidRenderer = {
  name: "LiquidJs",
  template: ".my-btn { {%- for v in value %}{{ v }}{%- endfor%} }",
};

const testComponentRenderer: ILiquidRenderer = {
  name: "LiquidJs",
  template: "color: {{ value.hex }};",
};

const buttonColorScheme: ISchemaComponent = {
  component: "Color",
  displayText: "Button text color",
  hint: "Pick a color for the button text",
  defaultValue: defaultValue,
  renderer: testComponentRenderer,
};

const testButtonSchema = {
  Button: {
    displayText: "The whole button (nested selectors)",
    hint: "The whole button",
    renderer: testSchemaRenderer,
    example: "<button class='btn my-btn'>Default</button",
    schema: {
      ButtonColor: buttonColorScheme,
    },
  },
};

// TODO we might put a props optional value here to configure components.

// Anything that goes into a schema must consist of this
// may have no need to export this.
interface ISchemaEntryBase {
  displayText: string;
  hint: string;
  renderer: ILiquidRenderer | string;
}

/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
interface ISchemaComponentProps extends Record<string, any> {
  [x: string]: any;
}

// A direct component schema
interface ISchemaComponent extends ISchemaEntryBase {
  component: string;
  defaultValue: ISchemaComponentValue;
  props?: ISchemaComponentProps;
}

// A nested schema which must contain either more schemas
// or components
interface ISchemaContainer extends ISchemaEntryBase {
  example: string;
  schema: ISchema;
}

// maybe adds confusiong with StyleRecord
interface ISchema extends Record<string, ISchemaContainer | ISchemaComponent> {
  [x: string]: ISchemaContainer | ISchemaComponent;
}

/* eslint-disable @typescript-eslint/no-empty-interface*/
interface ISchemaComponentValue extends IDynamic {}

/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
const isOfTypeSchema = (instance: any) => {
  return isOfType<ISchema>(instance, "schema");
};

interface ILiquidRenderer {
  name: string;
  template: string;
}

export {
  ISchemaComponentValue,
  ISchemaComponentProps,
  ILiquidRenderer,
  ISchema,
  ISchemaContainer,
  ISchemaComponent,
  isOfTypeSchema,
};

const props = {
  units: [
    {
      displayText: "Rem",
      value: "rem",
    },
    {
      displayText: "Pixels",
      value: "px",
    },
  ],
  mergeUnits: false,
};

// eslint-disable-line @typescript-eslint/no-unused-vars
const borderRadius: ISchemaComponent = {
  component: "TextSizeSingle",
  displayText: "Border radius",
  hint: "Enter a border radius (rem, em, px)",
  defaultValue: {
    text: {
      text: ".5",
      unit: "rem",
    },
    compiled: "",
  },
  props: props,
  renderer: "CssSizeEntry",
};

// eslint-disable-line @typescript-eslint/no-unused-vars
const schemaComponent: ISchemaComponent = {
  component: "Color",
  displayText: "Button text color",
  hint: "Pick a color for the button text",
  defaultValue: {
    color: {
      hex: "#8bc34a",
      rgb: "rgba(139, 195, 74, 1)",
    },
  },
  renderer: {
    name: "LiquidJs",
    template: "color: {{ value.hex }};",
  },
};

// eslint-disable-line @typescript-eslint/no-unused-vars
const schemaComponent2: ISchemaComponent = {
  component: "Color",
  displayText: "Button text color",
  hint: "Pick a color for the button text",
  defaultValue: {
    color: {
      hex: "#8bc34a",
      rgb: "rgba(139, 195, 74, 1)",
    },
  },
  renderer: "CssColorVar",
};

// export type ReadingTypes = 'some'|'variants'|'of'|'strings';

// export interface IReadings {
//    param:ReadingTypes
// }
// https://stackoverflow.com/questions/33836671/typescript-interface-that-allows-other-properties
//https://stackoverflow.com/questions/38245081/nested-objects-in-typescript
