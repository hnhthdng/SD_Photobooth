export type FieldType = "text" | "select" | "switch" | "number" | "date";

export interface OptionItem {
  label: string;
  value: string;
}

export interface FieldConfig {
  name: string;
  label: string;
  type: FieldType;
  options?: OptionItem[];
}
