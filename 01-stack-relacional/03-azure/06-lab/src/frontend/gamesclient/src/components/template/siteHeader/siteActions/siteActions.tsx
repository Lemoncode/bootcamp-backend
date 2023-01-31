import { useTranslation } from 'react-i18next';
import { useTheme } from 'react-jss';
// import { useAppSelector } from '../../../../../redux/hooks';
// import { SiteActionsStyles } from './siteActions.jss';

export const SiteActions = () => {
    const theme = useTheme();
    // const styles = SiteActionsStyles({ theme });
    const { t } = useTranslation('common', { keyPrefix: 'SITE.SITEHEADER' });
    // const { name } = useAppSelector((store) => store.common.userProfile);

    return (
        <>
            <div>Actions placeholder</div>
            {/* <div className={styles.siteActionsContainer}>{t('WELCOME_MESSAGE', { user: name })}</div> */}
        </>
    );
};
