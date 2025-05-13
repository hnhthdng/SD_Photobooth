import React from 'react'
import LocaleSelect from './LocaleSelect';
import { useLocale, useTranslations } from 'next-intl';

export default function ButtonLanguage  ()  {
  const t = useTranslations('ButtonTest');
  const locale = useLocale();
  return (
    <LocaleSelect
      defaultValue={locale}
      items={[
        {
          value: 'en',
          label: t('en'),
          image: '/united-kingdom.png'
        },
        {
          value: 'vi',
          label: t('vi'),
          image: '/vietnam.png'
        }
      ]}
      label={t('label')}
    />
  )
}


