import { memo, ReactNode } from "react";
import { Dialog, DialogContent, DialogTrigger } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import FormRenderer from "./FormRenderer";
import { FieldConfig } from "@/types/field-config";

type CreateDialogFormProps = {
  title: string;
  fields: FieldConfig[];
  schema: z.ZodType<any>;
  onSubmit: (data: any) => Promise<void>;
  triggerIcon: ReactNode;
  triggerText?: string;
};

function CreateDialogForm({
  title,
  fields,
  schema,
  onSubmit,
  triggerIcon,
  triggerText,
}: CreateDialogFormProps) {
  const form = useForm({
    resolver: zodResolver(schema),
  });

  const onSubmitInternal = async (data: any) => {
    await onSubmit(data);
    form.reset();
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button>
          {triggerIcon}
          {triggerText && <span className="ml-2">{triggerText}</span>}
        </Button>
      </DialogTrigger>
      <DialogContent>
        <h2 className="text-xl font-semibold mb-4">{title}</h2>
        <FormProvider {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmitInternal)}
            className="space-y-4"
          >
            <FormRenderer fields={fields} />
            <Button type="submit">Submit</Button>
          </form>
        </FormProvider>
      </DialogContent>
    </Dialog>
  );
}

export const MemoizedCreateDialogForm = memo(CreateDialogForm);
