import { createUseStyles } from 'react-jss';

export const SiteHeaderStyles = createUseStyles((theme: any) => ({
    siteHeader: {
        display: 'flex',
        flexFlow: 'row nowrap',
        justifyContent: 'space-between',
        alignItems: 'center',
        background: theme.colors.site.header.background,
        padding: theme.paddings.site.header,
        gap: theme.gaps.default.rowGap,
        rowGap: 'row-gap',
        height: theme.heights.siteHeader,
        boxSizing: 'border-box',
        color: theme.colors.site.header.color,
    },
}));
