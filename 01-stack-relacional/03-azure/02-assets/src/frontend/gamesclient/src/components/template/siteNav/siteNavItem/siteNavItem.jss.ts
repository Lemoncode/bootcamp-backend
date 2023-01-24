import { createUseStyles } from 'react-jss';

export const SiteNavItemStyles = createUseStyles((theme: any) => ({
    item: {
        display: 'flex',
        columnGap: theme.gaps.default.columnGap,
        padding: theme.paddings.site.nav.item,
        borderRadius: theme.borders.site.nav.item.radius,
        fontWeight: 'bold',
        textDecoration: 'none',
        color: theme.colors.site.nav.item.color,
        boxSizing: 'border-box',
        '&:hover': {
            color: `${theme.colors.site.nav.item.hover.color} !important`,
        },
    },
    itemActive: {
        display: 'flex',
        padding: theme.paddings.site.nav.item,
        fontWeight: 'bold',
        textDecoration: 'none',
        color: theme.colors.site.nav.item.active.color,
        background: theme.colors.site.nav.item.active.background,
        columnGap: theme.gaps.default.columnGap,
        borderRadius: theme.borders.site.nav.item.radius,
        boxSizing: 'border-box',
    },
    itemActiveSeparator: {
        borderLeft: `4px solid ${theme.colors.site.nav.item.hover.color}`,
        height: 16,
        display: 'flex',
        alignSelf: 'center',
    },
}));
