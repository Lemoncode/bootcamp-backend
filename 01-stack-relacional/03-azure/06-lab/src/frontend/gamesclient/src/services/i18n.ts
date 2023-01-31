import i18n from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import { initReactI18next } from 'react-i18next';
import common from '../translations/es/common.json';
import common_en from '../translations/en/common.json';

export const resources = {
    es: {
        common,
    },
    en: {
        common,
    },
} as const;

i18n.use(LanguageDetector)
    .use(initReactI18next)
    .init({
        fallbackLng: 'es',
        ns: ['common'],
        defaultNS: 'common',
        interpolation: {
            format: function (value, format, lng) {
                if (format === 'fullDate') {
                    return Intl.DateTimeFormat(lng, { dateStyle: 'full' }).format(value);
                }
                if (format === 'dayName') {
                    return Intl.DateTimeFormat(lng, { weekday: 'long' }).format(value);
                }
                if (format === 'monthName') {
                    return Intl.DateTimeFormat(lng, { month: 'long' }).format(value);
                }
                if (format === 'dayWithZeros') {
                    return Intl.DateTimeFormat(lng, { day: '2-digit' }).format(value);
                }
                return value;
            },
        },
        resources: {
            es: {
                common: common,
            },
            en: {
                common: common_en,
            },
        },
        debug: false,
        detection: {
            order: ['querystring', 'cookie', 'localStorage', 'navigator'],
            lookupQuerystring: 'lng',
            lookupCookie: 'i18next',
            lookupLocalStorage: 'i18nextLng',
            caches: ['localStorage', 'cookie'],
        },
    });

export default i18n;
