import { IDynamic } from "./dynamic"

const testStyleSchemaRecord: IStyleRecord = {
  Button: {
    value: {
      ButtonColor: {
        value: {
          color: {
            hex: "#596b45",
            rgb: "rgb(89 107 69)"
          },
          compiled: "color: #596b45;"
        }
      }
    },
    compiled: "all of schema"
  }
}

// so it does work when it is nested, it's just a bit weird because it supports nesting.
const testStyleComponentRecord: IStyleRecord = {
  ButtonColor: {
    value: {
      value: {
        color: {
          hex: "#596b45",
          rgb: "rgb(89 107 69)"
        },
        compiled: "color: #596b45;"
      }
    },
    compiled: ""
  },
}

// this is good.
const testStyleValue: IStyleRecordComponentValue = {
  color: {
    hex: "#596b45",
    rgb: "rgb(89 107 69)"
  },
  compiled: "color: #596b45;"
}

const buttonColorComponent: IStyleRecordComponent = {
  value: testStyleValue
}

const buttonColorRecord: IStyleRecordContainer = {
  value: {
    ButtonColor: buttonColorComponent,
    ButtonColor2: buttonColorComponent
  },
  compiled: "all of schema"
}

const testStyleRecordValue2: IStyleRecordComponentValue = {
  value: {
    Button: buttonColorRecord
  },
  compiled: "color: #596b45;"
}

interface ICompiledStyle {
  compiled: string
}

interface IStyleRecordContainer extends ICompiledStyle {
  value: IStyleRecordComponent | Record<string, IStyleRecordComponent>,
}

interface IStyleRecordComponent {
  value: IStyleRecordComponentValue
};

// OK This is right for a component value.
// it must have a compiled output and it must have another key for color, or text etc.
interface IStyleRecordComponentValue extends ICompiledStyle, IDynamic {
}

interface IStyleRecordOutput  {
  schemaKey: string;
  value: IStyleRecordComponentValue;
}

interface IStyleRecord extends Record<string, IStyleRecordContainer | IStyleRecord> {
  [x: string]: IStyleRecordContainer | IStyleRecord
}

export {
  ICompiledStyle,
  IStyleRecordOutput,
  IStyleRecordComponentValue,
  IStyleRecordComponent,
  IStyleRecordContainer,
  IStyleRecord
}