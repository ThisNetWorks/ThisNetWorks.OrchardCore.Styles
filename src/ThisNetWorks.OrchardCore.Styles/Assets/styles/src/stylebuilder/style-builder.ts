import {
  ISchema,
  ISchemaComponent,
  ISchemaComponentValue,
  ISchemaContainer,
  isOfTypeSchema,
} from "@/types/schema";
import { IStyleRecord } from "@/types/style-record";

const buildStyleRecord = (
  schema: ISchema,
  styleRecord: IStyleRecord
): IStyleRecord => {
  const schemaStyle = Object.fromEntries(
    Object.entries(schema).map(([k, v]) => {
      const isSchema = isOfTypeSchema(v);
      if (isSchema) {
        const currentSchema = (v as ISchemaContainer)?.schema;
        const currentStyleRecord = styleRecord[k];
        if (currentStyleRecord) {
          return [
            k,
            buildStyleRecord(currentSchema, currentStyleRecord as IStyleRecord),
          ];
        }

        return [k, buildStyleRecord(currentSchema, {})];
      } else {
        const currentStyleRecord = styleRecord[k];
        if (currentStyleRecord) {
          return [k, currentStyleRecord];
        }

        const currentSchemaComponent = v as ISchemaComponent;
        const newComponent: ISchemaComponentValue = {
          ...currentSchemaComponent.defaultValue,
          compiled: "",
        };
        return [k, newComponent];
      }
    })
  );

  return schemaStyle;
};

export { buildStyleRecord };
