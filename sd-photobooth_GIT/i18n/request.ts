import { getUserLocale } from '@/services/locale';
import {getRequestConfig} from 'next-intl/server';
 
const SUPPORTED_LOCALES = ['vi', 'en'];
export default getRequestConfig(async () => {
  // Provide a static locale, fetch a user setting,
  // read from `cookies()`, `headers()`, etc.
  let locale = await getUserLocale();
  if (!SUPPORTED_LOCALES.includes(locale)) {
    locale = 'en'; // Hoặc 'vi', tuỳ bạn
  }
  return {
    locale,
    messages: (await import(`../messages/${locale}.json`)).default
  };
});