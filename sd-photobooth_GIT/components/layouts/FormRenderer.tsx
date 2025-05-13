import { useFormContext } from "react-hook-form";
import { Input } from "@/components/ui/input";
import { Switch } from "@/components/ui/switch";
import {
  Select,
  SelectTrigger,
  SelectContent,
  SelectItem,
} from "@/components/ui/select";
import { FieldConfig } from "@/types/field-config";

export default function FormRenderer({ fields }: { fields: FieldConfig[] }) {
  const { register, setValue, watch,formState: { errors } } = useFormContext();

  return (
    <>
      {fields.map((field) => {
        const value = watch(field.name);
        const error = errors[field.name];
        switch (field.type) {
          case "text":
          case "number":
            return (
              <div key={field.name}>
                <label className="block mb-1">{field.label}</label>
                <Input type={field.type} {...register(field.name)} />
              </div>
            );

          case "select":
            return (
              <div key={field.name}>
                <label className="block mb-1">{field.label}</label>
                <Select
                  value={value}
                  onValueChange={(val) => setValue(field.name, val)}
                >
                  <SelectTrigger />
                  <SelectContent>
                    {field.options?.map((opt) => (
                      <SelectItem key={opt.label} value={opt.value}>
                        {opt.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
            );

          case "switch":
            return (
              <div
                key={field.name}
                className="flex items-center justify-between"
              >
                <label>{field.label}</label>
                <Switch
                  checked={value}
                  onCheckedChange={(val) => setValue(field.name, val)}
                />
              </div>
            );

          default:
            return null;
        }
      })}
    </>
  );
}
