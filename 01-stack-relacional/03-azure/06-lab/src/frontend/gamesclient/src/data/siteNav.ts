import { routeUrls } from '../constants';
import { INavItem } from '../models/sitenav/INavItem';

export const siteNavData: INavItem[] = [
    {
        title: 'common:SITE.SITENAV.HOME',
        link: routeUrls.HOME,
        enabled: true,
    },
    // {
    //     title: 'common:SITE.SITENAV.HOME',
    //     link: routeUrls.HOME,
    //     enabled: true,
    // },
    // {
    //     title: 'example of target _blank, not enabled',
    //     icon: 'blog',
    //     link: 'http://blog.santiagoporras.com',
    //     enabled: false,
    //     target: '_blank',
    // },
];

