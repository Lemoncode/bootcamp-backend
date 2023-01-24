import { createUseStyles } from 'react-jss';

export const SiteActionsStyles = createUseStyles((theme: any) => ({
    siteActionsContainer: {
        display: 'flex',
        color: theme.colors.site.header.title,
        fontSize: theme.fonts.sizes.title.module,
        fontWeight: 300,
    },
}));
