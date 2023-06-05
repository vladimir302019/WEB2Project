import React, { useEffect, useState } from 'react'
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardMedia from '@mui/material/CardMedia';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { TextField } from '@mui/material';
import { getAllArticlesAction } from '../../Store/articleSlice';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

export default function Products({cartArticles, addArticlesToCart}) {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const articles = useSelector((state) => state.article.allArticles);
    const [isInitial, setIsInitial] = useState(true);
    const [articlesForCart, setArticlesForCart] = useState([...articles.map(article => ({...article, quantity: 0}))]);
    const img = "data:image/*;base64,";
    const imgUrl = process.env.PUBLIC_URL +'/no-image.png';

    const handleAddArticle = (articleId) => {
        const article = articlesForCart.find(article => {return article.id === articleId;});
        addArticlesToCart(article);
    } 

    const handleQuantityChange = (articleId, newQuantity) => {
        const updatedArticles = articlesForCart.map((article) => {
            if(article.id === articleId){
                return {...article, quantity: newQuantity};
            }
            return article;
        });
        setArticlesForCart(updatedArticles);
    }

    useEffect(()=> {
        if(!isInitial) {
          return;
        }
    
        const execute = async () => {
          await dispatch(getAllArticlesAction());
        };
    
        execute();
        setIsInitial(false);
    
      }, [isInitial, dispatch]);
      if(!articles){
        return <div>Loading ...</div>
      }else
  return (
    <Container sx={{ py: 3 }}>
    <Grid container>
        {articles.map((article) => (
        <Grid item key={article.id} xs={12} sm={12} md={12} mt={2}>
            <Card
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'flex-start',
                padding: '10px',
            }}
            >
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }}>
                <CardMedia
                component="img"
                src={article.photoUrl !== "" ? img+article.photoUrl : imgUrl}
                alt="Product Image"
                sx={{ width: '30%', height: 'auto', marginRight: '16px' }}
                />
                <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-start', marginLeft: 1 }}>
                <Typography gutterBottom variant="h5" component="h2" >
                    {article.name}
                </Typography>
                <Typography>
                    {article.description}
                </Typography>
                </div>
                <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end', marginLeft: 3}}>
                <Typography>
                    Price: {article.price}.00 din
                </Typography>
                <TextField type="number" variant="outlined" size="small" sx={{ marginBottom: 2 }} inputProps={{max:article.maxQuantity, min: 0}}
                    onChange={(e) => handleQuantityChange(article.id, parseInt(e.target.value))}/>
                <Button variant="contained" size="medium" color="primary" onClick={() => handleAddArticle(article.id)}>
                    Add
                </Button>
                </div>
            </div>
            </Card>
        </Grid>
        ))}
    </Grid>
    </Container>
  )
}