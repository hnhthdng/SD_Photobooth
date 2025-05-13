import React, { useTransition } from 'react'

import * as Select from '@radix-ui/react-select';
import clsx from 'clsx';
import { GrLanguage } from "react-icons/gr";
import { Locale } from '@/i18n/config';
import { setUserLocale } from '@/services/locale';
import { MdOutlineRadioButtonChecked } from "react-icons/md";
import Image from 'next/image';
type Props = {
  defaultValue: string;
  items: Array<{ value: string; label: string; image?: string }>;
  label: string;
};
const LocaleSelect = ({
  defaultValue,
  items,
  label
}: Props) => {
  const [isPending, startTransition] = useTransition();
  function onChange(value: string) {
    const locale = value as Locale;
    startTransition(() => {
      setUserLocale(locale);
    });
  }
  return (
    <div className="relative">
      <Select.Root defaultValue={defaultValue} onValueChange={onChange}>
        <Select.Trigger
          aria-label={label}
          className={clsx(
            ' rounded-full  bg-white border border-gray-300 hover:bg-gray-200 p-2 transition-colors dark:border-gray-700 dark:bg-black  dark:hover:bg-gray-700 ',
            isPending && 'pointer-events-none opacity-60'
          )}
        >
          <Select.Icon>
            <GrLanguage className="h-6 w-6 text-slate-600 dark:text-slate-400 transition-colors dark:group-hover:text-slate-900 " />

          </Select.Icon>
        </Select.Trigger>
        <Select.Portal>
          <Select.Content
            align="end"
            className="min-w-[10rem]  overflow-hidden rounded-sm bg-white py-1 shadow-md border border-gray-300 dark:border-gray-700 dark:bg-black   "
            position="popper"
          >
            <Select.Viewport>
              {items.map((item) => (
                <Select.Item

                  key={item.value}
                  className="flex cursor-default items-center px-3 py-2 text-base data-[highlighted]:bg-slate-200 dark:data-[highlighted]:bg-slate-800"
                  value={item.value}
                >

                  {item.image && (
                    <Image src={item.image} alt={item.label} className=" rounded-full mr-2" width={24}
                      height={24} />
                  )}
                  <span className="text-slate-900 dark:text-white ">{item.label}</span>

                  <div className="ml-auto w-[1rem]">
                    {item.value === defaultValue && (
                      <MdOutlineRadioButtonChecked className=" text-slate-600" />
                    )}
                  </div>

                </Select.Item>
              ))}
            </Select.Viewport>
            <Select.Arrow className="fill-gray-300 dark:fill-gray-500 text-white" />
          </Select.Content>
        </Select.Portal>
      </Select.Root>
    </div>
  )
}

export default LocaleSelect
